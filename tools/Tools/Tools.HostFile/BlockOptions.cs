// ***********************************************************************
// Assembly         : Tools.HostFile
// Author           : @tysmithnet
// Created          : 10-13-2018
//
// Last Modified By : @tysmithnet
// Last Modified On : 10-13-2018
// ***********************************************************************
// <copyright file="BlockOptions.cs" company="">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************

using CommandLine;

namespace Tools.HostFile
{
    /// <summary>
    ///     Class BlockOptions.
    /// </summary>
    /// <seealso cref="Tools.HostFile.FacadeOptions" />
    [Verb("block", HelpText = "Block domains by setting their IP to loopback")]
    public class BlockOptions : FacadeOptions
    {
    }
}