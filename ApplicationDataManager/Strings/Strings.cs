#pragma checksum "C:\Users\liuzh\Source\Repos\ExViewer\ApplicationDataManager\Strings\en\Resources.resJson" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "222A55C8501A12A7048F7285CEFADC93"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace ApplicationDataManager.ApplicationDataManager_ResourceInfo
{
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Opportunity.ResourceGenerator.Generator", "1.0.1.0")]
    public interface IResources : global::Opportunity.ResourceGenerator.IResourceProvider
    {
        global::ApplicationDataManager.ApplicationDataManager_ResourceInfo.Resources.IBoolean Boolean { get; }
    }
}

namespace ApplicationDataManager.ApplicationDataManager_ResourceInfo.Resources
{
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Opportunity.ResourceGenerator.Generator", "1.0.1.0")]
    public interface IBoolean : global::Opportunity.ResourceGenerator.IResourceProvider
    {
        global::ApplicationDataManager.ApplicationDataManager_ResourceInfo.Resources.Boolean.IYesNo YesNo { get; }
        global::ApplicationDataManager.ApplicationDataManager_ResourceInfo.Resources.Boolean.IEnabledDisabled EnabledDisabled { get; }
        global::ApplicationDataManager.ApplicationDataManager_ResourceInfo.Resources.Boolean.IOnOff OnOff { get; }
        global::ApplicationDataManager.ApplicationDataManager_ResourceInfo.Resources.Boolean.ITrueFalse TrueFalse { get; }
    }
}

namespace ApplicationDataManager.ApplicationDataManager_ResourceInfo.Resources.Boolean
{
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Opportunity.ResourceGenerator.Generator", "1.0.1.0")]
    public interface IYesNo : global::Opportunity.ResourceGenerator.IResourceProvider
    {
        /// <summary>
        /// <para>Yes</para>
        /// </summary>
        string True { get; }
        /// <summary>
        /// <para>No</para>
        /// </summary>
        string False { get; }
    }
}

namespace ApplicationDataManager.ApplicationDataManager_ResourceInfo.Resources.Boolean
{
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Opportunity.ResourceGenerator.Generator", "1.0.1.0")]
    public interface IEnabledDisabled : global::Opportunity.ResourceGenerator.IResourceProvider
    {
        /// <summary>
        /// <para>Enabled</para>
        /// </summary>
        string True { get; }
        /// <summary>
        /// <para>Disabled</para>
        /// </summary>
        string False { get; }
    }
}

namespace ApplicationDataManager.ApplicationDataManager_ResourceInfo.Resources.Boolean
{
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Opportunity.ResourceGenerator.Generator", "1.0.1.0")]
    public interface IOnOff : global::Opportunity.ResourceGenerator.IResourceProvider
    {
        /// <summary>
        /// <para>On</para>
        /// </summary>
        string True { get; }
        /// <summary>
        /// <para>Off</para>
        /// </summary>
        string False { get; }
    }
}

namespace ApplicationDataManager.ApplicationDataManager_ResourceInfo.Resources.Boolean
{
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Opportunity.ResourceGenerator.Generator", "1.0.1.0")]
    public interface ITrueFalse : global::Opportunity.ResourceGenerator.IResourceProvider
    {
        /// <summary>
        /// <para>True</para>
        /// </summary>
        string True { get; }
        /// <summary>
        /// <para>False</para>
        /// </summary>
        string False { get; }
    }
}

namespace ApplicationDataManager
{
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Opportunity.ResourceGenerator.Generator", "1.0.1.0")]
    public static class Strings
    {

        public static global::ApplicationDataManager.ApplicationDataManager_ResourceInfo.IResources Resources { get; } = new global::ApplicationDataManager.Strings.Resources___Mjf_Vzs();

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Opportunity.ResourceGenerator.Generator", "1.0.1.0")]
        [global::System.Diagnostics.DebuggerDisplay("[ms-resource:///ApplicationDataManager/Resources]")]
        private sealed class Resources___Mjf_Vzs : global::ApplicationDataManager.ApplicationDataManager_ResourceInfo.IResources
        {
            global::Opportunity.ResourceGenerator.GeneratedResourceProvider global::Opportunity.ResourceGenerator.IResourceProvider.this[string resourceKey]
            {
                get
                {
                    if(resourceKey == null)
                        throw new global::System.ArgumentNullException();
                    return new global::Opportunity.ResourceGenerator.GeneratedResourceProvider("ms-resource:///ApplicationDataManager/Resources/" + resourceKey);
                }
            }

            string global::Opportunity.ResourceGenerator.IResourceProvider.GetValue(string resourceKey)
            {
                return global::Opportunity.ResourceGenerator.LocalizedStrings.GetValue("ms-resource:///ApplicationDataManager/Resources/" + resourceKey);
            }


            global::ApplicationDataManager.ApplicationDataManager_ResourceInfo.Resources.IBoolean global::ApplicationDataManager.ApplicationDataManager_ResourceInfo.IResources.Boolean { get; } = new global::ApplicationDataManager.Strings.Resources___Mjf_Vzs.Boolean__6ZuHYFms();

            [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Opportunity.ResourceGenerator.Generator", "1.0.1.0")]
            [global::System.Diagnostics.DebuggerDisplay("[ms-resource:///ApplicationDataManager/Resources/Boolean]", Name = "Boolean")]
            private sealed class Boolean__6ZuHYFms : global::ApplicationDataManager.ApplicationDataManager_ResourceInfo.Resources.IBoolean
            {
                global::Opportunity.ResourceGenerator.GeneratedResourceProvider global::Opportunity.ResourceGenerator.IResourceProvider.this[string resourceKey]
                {
                    get
                    {
                        if(resourceKey == null)
                            throw new global::System.ArgumentNullException();
                        return new global::Opportunity.ResourceGenerator.GeneratedResourceProvider("ms-resource:///ApplicationDataManager/Resources/Boolean/" + resourceKey);
                    }
                }

                string global::Opportunity.ResourceGenerator.IResourceProvider.GetValue(string resourceKey)
                {
                    if(resourceKey == null)
                        return global::Opportunity.ResourceGenerator.LocalizedStrings.GetValue("ms-resource:///ApplicationDataManager/Resources/Boolean");
                    return global::Opportunity.ResourceGenerator.LocalizedStrings.GetValue("ms-resource:///ApplicationDataManager/Resources/Boolean/" + resourceKey);
                }

                global::ApplicationDataManager.ApplicationDataManager_ResourceInfo.Resources.Boolean.IYesNo global::ApplicationDataManager.ApplicationDataManager_ResourceInfo.Resources.IBoolean.YesNo { get; } = new global::ApplicationDataManager.Strings.Resources___Mjf_Vzs.Boolean__6ZuHYFms.YesNo__qsT7qQSI();

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Opportunity.ResourceGenerator.Generator", "1.0.1.0")]
                [global::System.Diagnostics.DebuggerDisplay("[ms-resource:///ApplicationDataManager/Resources/Boolean/YesNo]", Name = "YesNo")]
                private sealed class YesNo__qsT7qQSI : global::ApplicationDataManager.ApplicationDataManager_ResourceInfo.Resources.Boolean.IYesNo
                {
                    global::Opportunity.ResourceGenerator.GeneratedResourceProvider global::Opportunity.ResourceGenerator.IResourceProvider.this[string resourceKey]
                    {
                        get
                        {
                            if(resourceKey == null)
                                throw new global::System.ArgumentNullException();
                            return new global::Opportunity.ResourceGenerator.GeneratedResourceProvider("ms-resource:///ApplicationDataManager/Resources/Boolean/YesNo/" + resourceKey);
                        }
                    }

                    string global::Opportunity.ResourceGenerator.IResourceProvider.GetValue(string resourceKey)
                    {
                        if(resourceKey == null)
                            return global::Opportunity.ResourceGenerator.LocalizedStrings.GetValue("ms-resource:///ApplicationDataManager/Resources/Boolean/YesNo");
                        return global::Opportunity.ResourceGenerator.LocalizedStrings.GetValue("ms-resource:///ApplicationDataManager/Resources/Boolean/YesNo/" + resourceKey);
                    }

                    string global::ApplicationDataManager.ApplicationDataManager_ResourceInfo.Resources.Boolean.IYesNo.True
                        => global::Opportunity.ResourceGenerator.LocalizedStrings.GetValue("ms-resource:///ApplicationDataManager/Resources/Boolean/YesNo/True");
                    string global::ApplicationDataManager.ApplicationDataManager_ResourceInfo.Resources.Boolean.IYesNo.False
                        => global::Opportunity.ResourceGenerator.LocalizedStrings.GetValue("ms-resource:///ApplicationDataManager/Resources/Boolean/YesNo/False");
                }

                global::ApplicationDataManager.ApplicationDataManager_ResourceInfo.Resources.Boolean.IEnabledDisabled global::ApplicationDataManager.ApplicationDataManager_ResourceInfo.Resources.IBoolean.EnabledDisabled { get; } = new global::ApplicationDataManager.Strings.Resources___Mjf_Vzs.Boolean__6ZuHYFms.EnabledDisabled__CVhUaceJ();

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Opportunity.ResourceGenerator.Generator", "1.0.1.0")]
                [global::System.Diagnostics.DebuggerDisplay("[ms-resource:///ApplicationDataManager/Resources/Boolean/EnabledDisabled]", Name = "EnabledDisabled")]
                private sealed class EnabledDisabled__CVhUaceJ : global::ApplicationDataManager.ApplicationDataManager_ResourceInfo.Resources.Boolean.IEnabledDisabled
                {
                    global::Opportunity.ResourceGenerator.GeneratedResourceProvider global::Opportunity.ResourceGenerator.IResourceProvider.this[string resourceKey]
                    {
                        get
                        {
                            if(resourceKey == null)
                                throw new global::System.ArgumentNullException();
                            return new global::Opportunity.ResourceGenerator.GeneratedResourceProvider("ms-resource:///ApplicationDataManager/Resources/Boolean/EnabledDisabled/" + resourceKey);
                        }
                    }

                    string global::Opportunity.ResourceGenerator.IResourceProvider.GetValue(string resourceKey)
                    {
                        if(resourceKey == null)
                            return global::Opportunity.ResourceGenerator.LocalizedStrings.GetValue("ms-resource:///ApplicationDataManager/Resources/Boolean/EnabledDisabled");
                        return global::Opportunity.ResourceGenerator.LocalizedStrings.GetValue("ms-resource:///ApplicationDataManager/Resources/Boolean/EnabledDisabled/" + resourceKey);
                    }

                    string global::ApplicationDataManager.ApplicationDataManager_ResourceInfo.Resources.Boolean.IEnabledDisabled.True
                        => global::Opportunity.ResourceGenerator.LocalizedStrings.GetValue("ms-resource:///ApplicationDataManager/Resources/Boolean/EnabledDisabled/True");
                    string global::ApplicationDataManager.ApplicationDataManager_ResourceInfo.Resources.Boolean.IEnabledDisabled.False
                        => global::Opportunity.ResourceGenerator.LocalizedStrings.GetValue("ms-resource:///ApplicationDataManager/Resources/Boolean/EnabledDisabled/False");
                }

                global::ApplicationDataManager.ApplicationDataManager_ResourceInfo.Resources.Boolean.IOnOff global::ApplicationDataManager.ApplicationDataManager_ResourceInfo.Resources.IBoolean.OnOff { get; } = new global::ApplicationDataManager.Strings.Resources___Mjf_Vzs.Boolean__6ZuHYFms.OnOff__Smjsy0ec();

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Opportunity.ResourceGenerator.Generator", "1.0.1.0")]
                [global::System.Diagnostics.DebuggerDisplay("[ms-resource:///ApplicationDataManager/Resources/Boolean/OnOff]", Name = "OnOff")]
                private sealed class OnOff__Smjsy0ec : global::ApplicationDataManager.ApplicationDataManager_ResourceInfo.Resources.Boolean.IOnOff
                {
                    global::Opportunity.ResourceGenerator.GeneratedResourceProvider global::Opportunity.ResourceGenerator.IResourceProvider.this[string resourceKey]
                    {
                        get
                        {
                            if(resourceKey == null)
                                throw new global::System.ArgumentNullException();
                            return new global::Opportunity.ResourceGenerator.GeneratedResourceProvider("ms-resource:///ApplicationDataManager/Resources/Boolean/OnOff/" + resourceKey);
                        }
                    }

                    string global::Opportunity.ResourceGenerator.IResourceProvider.GetValue(string resourceKey)
                    {
                        if(resourceKey == null)
                            return global::Opportunity.ResourceGenerator.LocalizedStrings.GetValue("ms-resource:///ApplicationDataManager/Resources/Boolean/OnOff");
                        return global::Opportunity.ResourceGenerator.LocalizedStrings.GetValue("ms-resource:///ApplicationDataManager/Resources/Boolean/OnOff/" + resourceKey);
                    }

                    string global::ApplicationDataManager.ApplicationDataManager_ResourceInfo.Resources.Boolean.IOnOff.True
                        => global::Opportunity.ResourceGenerator.LocalizedStrings.GetValue("ms-resource:///ApplicationDataManager/Resources/Boolean/OnOff/True");
                    string global::ApplicationDataManager.ApplicationDataManager_ResourceInfo.Resources.Boolean.IOnOff.False
                        => global::Opportunity.ResourceGenerator.LocalizedStrings.GetValue("ms-resource:///ApplicationDataManager/Resources/Boolean/OnOff/False");
                }

                global::ApplicationDataManager.ApplicationDataManager_ResourceInfo.Resources.Boolean.ITrueFalse global::ApplicationDataManager.ApplicationDataManager_ResourceInfo.Resources.IBoolean.TrueFalse { get; } = new global::ApplicationDataManager.Strings.Resources___Mjf_Vzs.Boolean__6ZuHYFms.TrueFalse__WEH8ctCi();

                [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
                [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Opportunity.ResourceGenerator.Generator", "1.0.1.0")]
                [global::System.Diagnostics.DebuggerDisplay("[ms-resource:///ApplicationDataManager/Resources/Boolean/TrueFalse]", Name = "TrueFalse")]
                private sealed class TrueFalse__WEH8ctCi : global::ApplicationDataManager.ApplicationDataManager_ResourceInfo.Resources.Boolean.ITrueFalse
                {
                    global::Opportunity.ResourceGenerator.GeneratedResourceProvider global::Opportunity.ResourceGenerator.IResourceProvider.this[string resourceKey]
                    {
                        get
                        {
                            if(resourceKey == null)
                                throw new global::System.ArgumentNullException();
                            return new global::Opportunity.ResourceGenerator.GeneratedResourceProvider("ms-resource:///ApplicationDataManager/Resources/Boolean/TrueFalse/" + resourceKey);
                        }
                    }

                    string global::Opportunity.ResourceGenerator.IResourceProvider.GetValue(string resourceKey)
                    {
                        if(resourceKey == null)
                            return global::Opportunity.ResourceGenerator.LocalizedStrings.GetValue("ms-resource:///ApplicationDataManager/Resources/Boolean/TrueFalse");
                        return global::Opportunity.ResourceGenerator.LocalizedStrings.GetValue("ms-resource:///ApplicationDataManager/Resources/Boolean/TrueFalse/" + resourceKey);
                    }

                    string global::ApplicationDataManager.ApplicationDataManager_ResourceInfo.Resources.Boolean.ITrueFalse.True
                        => global::Opportunity.ResourceGenerator.LocalizedStrings.GetValue("ms-resource:///ApplicationDataManager/Resources/Boolean/TrueFalse/True");
                    string global::ApplicationDataManager.ApplicationDataManager_ResourceInfo.Resources.Boolean.ITrueFalse.False
                        => global::Opportunity.ResourceGenerator.LocalizedStrings.GetValue("ms-resource:///ApplicationDataManager/Resources/Boolean/TrueFalse/False");
                }

            }

        }
    }
}