﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Windows.Foundation.Collections;
using Windows.Storage;

namespace ExViewer.Settings
{

    public class SettingCollectionBase : ExClient.ObservableObject
    {
        private bool loaded;
        private object syncroot = new object();

        private void load()
        {
            if(loaded)
                return;
            lock(syncroot)
            {
                if(loaded)
                    return;
                loaded = true;
                groupedProperties = (from property in this.GetType().GetRuntimeProperties()
                                     where property.GetCustomAttribute<SettingAttribute>() != null
                                     select new SettingInfo(property) into setting
                                     orderby setting.Index
                                     group setting by setting.Category into grouped
                                     select new GroupedSettings(grouped.Key, grouped)).ToList();
#if DEBUG
                testingProperties = (from property in this.GetType().GetRuntimeProperties()
                                     let t = property.GetCustomAttribute<TestingValueAttribute>()
                                     where t != null
                                     select Tuple.Create(property.Name, t.Value)).ToDictionary(p => p.Item1, p => p.Item2);
#endif
            }
        }
        
        private List<GroupedSettings> groupedProperties;

#if DEBUG
        private Dictionary<string, object> testingProperties;
#endif

        public List<GroupedSettings> GroupedSettings => groupedProperties;

        private IPropertySet local = ApplicationData.Current.LocalSettings.Values;
        private IPropertySet roaming = ApplicationData.Current.RoamingSettings.Values;

        private T Get<T>(IPropertySet container, T def, string key)
        {
            load();
#if DEBUG
            if(testingProperties.ContainsKey(key))
                return (T)testingProperties[key];
#endif
            try
            {
                object v;
                if(container.TryGetValue(key, out v))
                {
                    if(def is Enum)
                        return (T)Enum.Parse(typeof(T), v.ToString());
                    return (T)v;
                }
            }
            catch { }
            return def;
        }

        private bool HasValue(IPropertySet container, string key)
        {
            load();
#if DEBUG
            if(testingProperties.ContainsKey(key))
                return true;
#endif
            return container.ContainsKey(key);
        }

        private void Set<T>(IPropertySet container, T value, string key, bool forceRaiseEvent)
        {
            load();
            if(!forceRaiseEvent && HasValue(container, key) && Equals(Get(container, value, key), value))
                return;
            var enu = value as Enum;
            if(enu != null)
                container[key] = enu.ToString();
            else
                container[key] = value;
            RaisePropertyChanged(key);
        }

        protected T GetLocal<T>(string key)
        {
            return GetLocal(default(T), key);
        }

        protected T GetLocal<T>(T def, [CallerMemberName]string key = null)
        {
            return Get(local, def, key);
        }

        protected void SetLocal<T>(T value, [CallerMemberName]string key = null)
        {
            Set(local, value, key, false);
        }

        protected void ForceSetLocal<T>(T value, [CallerMemberName]string key = null)
        {
            Set(local, value, key, true);
        }

        protected T GetRoaming<T>(string key)
        {
            return GetRoaming(default(T), key);
        }

        protected T GetRoaming<T>(T def, [CallerMemberName]string key = null)
        {
            return Get(roaming, def, key);
        }

        protected void SetRoaming<T>(T value, [CallerMemberName]string key = null)
        {
            Set(roaming, value, key, false);
        }

        protected void ForceSetRoaming<T>(T value, [CallerMemberName]string key = null)
        {
            Set(roaming, value, key, true);
        }
    }
}