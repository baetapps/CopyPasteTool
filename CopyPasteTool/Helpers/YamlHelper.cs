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
   
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace CopyPasteTool.Helpers
{
    public class YamlHelper<T>
    {
        /// <summary>
        /// Gets the specified yml path.
        /// </summary>
        /// <param name="ymlPath">The yml path.</param>
        /// <returns></returns>
        public T Get(string ymlPath)
        {
            var source = File.ReadAllText(ymlPath);
            var deserializer = new Deserializer(namingConvention: new CamelCaseNamingConvention());

            using (var stringReader = new StringReader(source))
            {
                return deserializer.Deserialize<T>(stringReader);
            }
        }
    }
}