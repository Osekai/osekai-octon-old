﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Osekai.Octon.Database.EntityFramework.MySql {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class MySqlDataPopulatorResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal MySqlDataPopulatorResources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Osekai.Octon.Database.EntityFramework.MySql.MySqlDataPopulatorResources", typeof(MySqlDataPopulatorResources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT INTO `Apps` VALUES (-1,1,&apos;Home&apos;,&apos;home&apos;,1,0);
        ///INSERT INTO `AppThemes` VALUES (1,-1,&apos;53,61,85&apos;,1,1,&apos;53,61,85&apos;,1);
        ///INSERT INTO `Medals` VALUES (1,&apos;500 Combo&apos;,&apos;https://assets.ppy.sh/medals/web/osu-combo-500.png&apos;,&apos;500 big ones! You\&apos;re moving up in the world!&apos;,&apos;osu&apos;,&apos;Skill&apos;,&apos;aiming for a combo of 500 or higher on any beatmap&apos;,0,NULL,&apos;2008-08-02 00:00:00.000000&apos;,NULL,NULL),(3,&apos;750 Combo&apos;,&apos;https://assets.ppy.sh/medals/web/osu-combo-750.png&apos;,&apos;750 notes back to back? Woah.&apos;,&apos;osu&apos;,&apos;Skill&apos;,&apos;aiming for a combo [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Sql {
            get {
                return ResourceManager.GetString("Sql", resourceCulture);
            }
        }
    }
}
