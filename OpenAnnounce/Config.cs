﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace OpenAnnounce
{
    public static class Config
    {
        public static string Title { get { return ConfigurationManager.AppSettings["PageTitle"]; } }
    }
}