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
   
using CopyPasteTool.Messages;
using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;

namespace CopyPasteTool.View
{
    /// <summary>
    /// Interaction logic for CopyPasteView.xaml
    /// </summary>
    public partial class CopyPasteView : MetroWindow
    {
        public CopyPasteView()
        {
            InitializeComponent();
            Messenger.Default.Register<OpenDialogueMessage>(this, m => OpenDialogue(m));
        }

        /// <summary>
        /// Opens the dialogue.
        /// </summary>
        /// <param name="message">The message.</param>
        private void OpenDialogue(OpenDialogueMessage message)
        {
            this.Dispatcher.Invoke((Action)(() => { this.ShowMessageAsync(message.Title, message.Message); }));
        }
    }
}