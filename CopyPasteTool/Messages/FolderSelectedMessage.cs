﻿
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

using GalaSoft.MvvmLight.Messaging;
namespace CopyPasteTool.Messages
{
    public class FolderSelectedMessage : MessageBase
    {
        private readonly string selectedPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="FolderSelectedMessage"/> class.
        /// </summary>
        /// <param name="selectedPath">The selected path.</param>
        public FolderSelectedMessage(string selectedPath)
        {
            this.selectedPath = selectedPath;
        }

        /// <summary>
        /// Gets the selected path.
        /// </summary>
        /// <value>
        /// The selected path.
        /// </value>
        public string SelectedPath
        {
            get { return selectedPath; }
        }
    }
}