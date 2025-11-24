/* Copyright (c) Microsoft Corporation. All rights reserved.
   Licensed under the MIT License. */

using System;
using System.Globalization;
using log4net;

namespace RecurringIntegrationsScheduler.Common.Helpers
{
    public static class LogExtensions
    {
        public static void Verbose(this ILog log, bool verboseEnabled, Func<string> messageFactory)
        {
            if (log == null) throw new ArgumentNullException(nameof(log));
            if (messageFactory == null) throw new ArgumentNullException(nameof(messageFactory));

            if (!verboseEnabled && !log.IsDebugEnabled)
            {
                return;
            }

            var message = messageFactory();
            if (verboseEnabled)
            {
                log.Info(message);
            }
            else
            {
                log.Debug(message);
            }
        }

        public static void VerboseFormat(this ILog log, bool verboseEnabled, IFormatProvider provider, string format, params object[] args)
        {
            log.Verbose(verboseEnabled, () => string.Format(provider, format, args));
        }
    }
}
