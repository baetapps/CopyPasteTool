
/*
    The MIT License (MIT)
 
    Copyright (c) 2015 Leigh Shepperson
 
    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:
        
    The above copyright notice and this permission notice shall be included in
    all copies or substantial portions of the Software.
        
    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
    THE SOFTWARE.
        
    (http://opensource.org/licenses/mit-license.php)
*/
   
using CopyPasteTool.Helpers;
using CopyPasteTool.Messages;
using CopyPasteTool.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Input;

namespace CopyPasteTool.ViewModel
{
    public class CopyPasteViewModel : ViewModelBase, IDataErrorInfo
    {
        private readonly CodeDOMModel codeDOMModel = new YamlHelper<CodeDOMModel>().Get(@"Resources\CodeDOMModel.yml");

        private readonly Dictionary<string, string> validationErrors = new Dictionary<string, string>();
        private string exeName;
        private string selectedPath;
        private string text;

        /// <summary>
        /// Initializes a new instance of the <see cref="CopyPasteViewModel"/> class.
        /// </summary>
        public CopyPasteViewModel()
        {
            // Sets the selected folder from the folder selected message
            Messenger.Default.Register<FolderSelectedMessage>(this, DoSelectFolderCallback);

            // Condition: the file name and selected path is not null.
            CreateExeCommand = new RelayCommand(DoCreateExeCallBack, CanExecuteCreateExe);
        }

        /// <summary>
        /// Gets the create executable command.
        /// </summary>
        /// <value>
        /// The create executable command.
        /// </value>
        public ICommand CreateExeCommand { get; private set; }

        /// <summary>
        /// Gets or sets the name of the executable.
        /// </summary>
        /// <value>
        /// The name of the executable.
        /// </value>
        public string ExeName
        {
            get { return exeName; }
            set { Set(() => ExeName, ref exeName, value); }
        }

        /// <summary>
        /// Gets or sets the selected path.
        /// </summary>
        /// <value>
        /// The selected path.
        /// </value>
        public string SelectedPath
        {
            get { return selectedPath; }
            set { Set(() => SelectedPath, ref selectedPath, value); }
        }

        /// <summary>
        /// Gets or sets the text. This will be the text that is copied to the clipboard by the dynamic exe
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        public string Text
        {
            get { return text; }
            set { Set(() => Text, ref text, value); }
        }

        /// <summary>
        /// Determines whether this instance [can execute create executable].
        /// </summary>
        /// <returns>True iff ExeName and SelectedPath are not null or white space</returns>
        private bool CanExecuteCreateExe()
        {
            return !string.IsNullOrWhiteSpace(ExeName) && !string.IsNullOrWhiteSpace(SelectedPath);
        }

        /// <summary>
        /// Creates the copy paste executable.
        /// </summary>
        private void CreateCopyPasteExe()
        {
            // We use the CodeDomModel object to configure the CodeDOM provider
            using (var csc = new CSharpCodeProvider(codeDOMModel.ProviderOptions))
            {
                var path = Path.ChangeExtension(Path.Combine(SelectedPath, ExeName), ".exe");

                var parameters = new CompilerParameters(codeDOMModel.AssemblyNames, path, false);
                parameters.GenerateInMemory = true;
                parameters.GenerateExecutable = true;
                parameters.CompilerOptions = codeDOMModel.CompilerOptions;

                // Note, we are replacing single double quotes " with two double quotes "".
                var result = csc.CompileAssemblyFromSource(parameters, string.Format(codeDOMModel.ProgramBody, Text.Replace("\"", "\"\"")));

                if (result.Errors.HasErrors)
                {
                    var errorScript = new StringBuilder();

                    foreach (CompilerError error in result.Errors)
                    {
                        errorScript.AppendLine(string.Format("Error ({0}): {1}", error.ErrorNumber, error.ErrorText));
                    }

                    throw new InvalidOperationException(errorScript.ToString());
                }
            }
        }

        /// <summary>
        /// Does the create executable call back.
        /// </summary>
        private void DoCreateExeCallBack()
        {
            if (Validate())
            {
                try
                {
                    CreateCopyPasteExe();

                    MessengerInstance.Send<OpenDialogueMessage>(new OpenDialogueMessage() { Title = "Success...", Message = string.Format("The file {0} has been created at {1}", Path.ChangeExtension(ExeName, "exe"), SelectedPath) });
                }
                catch (InvalidOperationException ex)
                {
                    MessengerInstance.Send<OpenDialogueMessage>(new OpenDialogueMessage() { Title = "Error...", Message = ex.Message });
                }
            }
        }

        /// <summary>
        /// Does the select folder callback.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        private void DoSelectFolderCallback(FolderSelectedMessage msg)
        {
            SelectedPath = msg.SelectedPath;
        }

        /// <summary>
        /// Validates this instance.
        /// </summary>
        /// <returns>True iff there are no validation errors</returns>
        private bool Validate()
        {
            validationErrors.Clear();

            if (!Directory.Exists(SelectedPath))
            {
                validationErrors.Add("SelectedPath", "The selected folder does not exist.");
            }

            if (ExeName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
            {
                validationErrors.Add("ExeName", "The file name is invalid.");
            }

            // Forces the bindings to be reassesed so we can handle validation
            this.RaisePropertyChanged(string.Empty);

            return validationErrors.Count == 0;
        }

        #region IDataErrorInfo Members

        /// <summary>
        /// Gets an error message indicating what is wrong with this object.
        /// </summary>
        public string Error
        {
            get
            {
                if (validationErrors.Count > 0)
                {
                    return "Errors found.";
                }
                return null;
            }
        }

        /// <summary>
        /// Gets the error message for the property with the given name.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <returns>The error message associtated with this column.</returns>
        public string this[string columnName]
        {
            get
            {
                if (validationErrors.ContainsKey(columnName))
                {
                    return validationErrors[columnName];
                }
                return null;
            }
        }

        #endregion IDataErrorInfo Members
    }
}