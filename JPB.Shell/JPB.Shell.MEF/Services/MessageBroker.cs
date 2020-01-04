﻿
//By Jean-Pierre Bachmann
//http://www.codeproject.com/Articles/682642/Using-a-Plugin-Based-Application
//Questions? Ask me!
//Microsoft Public License (MS-PL)
/*
This license governs use of the accompanying software. If you use the software, you
accept this license. If you do not accept the license, do not use the software.

1. Definitions
The terms "reproduce," "reproduction," "derivative works," and "distribution" have the
same meaning here as under U.S. copyright law.
A "contribution" is the original software, or any additions or changes to the software.
A "contributor" is any person that distributes its contribution under this license.
"Licensed patents" are a contributor's patent claims that read directly on its contribution.

2. Grant of Rights
(A) Copyright Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, each contributor grants you a non-exclusive, worldwide, royalty-free copyright license to reproduce its contribution, prepare derivative works of its contribution, and distribute its contribution or any derivative works that you create.
(B) Patent Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, each contributor grants you a non-exclusive, worldwide, royalty-free license under its licensed patents to make, have made, use, sell, offer for sale, import, and/or otherwise dispose of its contribution in the software or derivative works of the contribution in the software.

3. Conditions and Limitations
(A) No Trademark License- This license does not grant you rights to use any contributors' name, logo, or trademarks.
(B) If you bring a patent claim against any contributor over patents that you claim are infringed by the software, your patent license from such contributor to the software ends automatically.
(C) If you distribute any portion of the software, you must retain all copyright, patent, trademark, and attribution notices that are present in the software.
(D) If you distribute any portion of the software in source code form, you may do so only under this license by including a complete copy of this license with your distribution. If you distribute any portion of the software in compiled or object code form, you may only do so under a license that complies with this license.
(E) The software is licensed "as-is." You bear the risk of using it. The contributors give no express warranties, guarantees or conditions. You may have additional consumer rights under your local laws which this license cannot change. To the extent permitted under your local laws, the contributors exclude the implied warranties of merchantability, fitness for a particular purpose and non-infringement.

All at all - Be carefull when using this code and do not remove the License!
*/

#region Jean-Pierre Bachmann

// Erstellt von Jean-Pierre Bachmann am 10:28

#endregion

using System;
using System.Collections.Generic;
using JPB.Shell.Contracts.Interfaces.Services.ShellServices;

namespace JPB.Shell.MEF.Services
{
    public class MessageBroker : IMessageBroker
    {
        private static MessageBroker _instance;
        private static readonly object LockObject = new object();
        private static Dictionary<Type, List<Delegate>> _receivers;

        private MessageBroker()
        {
            _receivers = new Dictionary<Type, List<Delegate>>();
        }

        /// <summary>
        ///     The Current MassageBroker Instance
        /// </summary>
        public static MessageBroker Instance
        {
            get
            {
                lock (LockObject)
                {
                    return _instance ?? (_instance = new MessageBroker());
                }
            }
        }

        #region IMessageBroker Members

        /// <summary>
        ///     Notify all Receiver that <typeparamref name="T" /> has been changed
        /// </summary>
        /// <typeparam name="T">Your targed type</typeparam>
        /// <param name="message">What your Receiver will get</param>
        public void Publish<T>(T message)
        {
            List<Delegate> list = null;
            if (_receivers.ContainsKey(typeof (T)))
            {
                list = _receivers[typeof (T)];
                foreach (var receiver in list)
                {
	                receiver.DynamicInvoke(message);
                }
            }
        }

        /// <summary>
        ///     Add a Receiver to the <see cref="MessageBroker" />
        /// </summary>
        /// <typeparam name="T">Your targed type</typeparam>
        /// <param name="callback">
        ///     A Callback that will used for a Notify if anyone will call <see cref="MessageBroker.Publish{T}" /> />
        /// </param>
        public void AddReceiver<T>(Action<T> callback)
        {
            List<Delegate> list = null;
            if (_receivers.ContainsKey(typeof (T)))
            {
	            list = _receivers[typeof (T)];
            }
            else
            {
                list = new List<Delegate>();
                _receivers.Add(typeof (T), list);
            }
            list.Add(callback);
        }

        #endregion
    }
}