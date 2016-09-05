using System.Collections.Generic;

namespace BigML
{
    /// <summary>
    /// A library is a special kind of compiled WhizzML source code that only
    /// defines functions and constants. It is intended as an import for
    /// executable scripts.
    /// The complete and updated reference with all available parameters is in
    /// our <a href="https://bigml.com/api/libraries">documentation</a>
    /// website.
    /// </summary>
    public partial class Library : Response
    {
        
        
        /// <summary>
        /// The name of the dataset as your provided or based on the name of the source by default.
        /// </summary>
        public string Name
        {
            get { return Object.name; }
        }

        /// <summary>
        /// The number of bytes of the source that were used to create this dataset.
        /// </summary>
        public int Size
        {
            get { return Object.size; }
        }

        /// <summary>
        /// The source/id that was used to build the dataset.
        /// </summary>
        public string SourceCode
        {
            get { return Object.sourceCode; }
        }


        /// <summary>
        /// A description of the status of the dataset.
        /// </summary>
        public Status StatusMessage
        {
            get{ return new Status(Object.status); }
        }
    }
}