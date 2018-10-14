// ***********************************************************************
// Assembly         : Tools.HostFile
// Author           : @tysmithnet
// Created          : 10-13-2018
//
// Last Modified By : @tysmithnet
// Last Modified On : 10-13-2018
// ***********************************************************************
// <copyright file="UnblockOptions.cs" company="">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************

using CommandLine;

namespace Tools.HostFile
{
    /// <summary>
    ///     Class UnblockOptions.
    /// </summary>
    /// <seealso cref="Tools.HostFile.FacadeOptions" />
    [Verb("unblock", HelpText = "Unblock any domains that have the IP set to loopback")]
    public class UnblockOptions : FacadeOptions
    {
    }
}