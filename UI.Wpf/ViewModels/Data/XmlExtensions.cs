using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Palmmedia.BackUp.Synchronization.SyncModes;

namespace Palmmedia.BackUp.UI.Wpf.ViewModels.Data
{
    /// <summary>
    /// Performs serialization and deserialization of <see cref="TasklistViewModel"/>.
    /// </summary>
    internal static class XmlExtensions
    {
        /// <summary>
        /// XML deserialization.
        /// </summary>
        /// <param name="tasklists">The tasklists.</param>
        /// <returns>The deserialized tasklists.</returns>
        public static IEnumerable<TasklistViewModel> FromXml(this XDocument tasklists)
        {
            return tasklists.Root.Elements("Task").Select(t => new TasklistViewModel(
                 t.Elements("TaskItem").Select(st => new SyncTaskViewModel(
                    new Synchronization.SyncTask(st.Attribute("Name").Value)
                    {
                        IsActive = (bool)st.Attribute("Active"),
                        ReferenceDirectory = st.Attribute("ReferenceDirectory").Value,
                        TargetDirectory = st.Attribute("TargetDirectory").Value,
                        Recursive = (bool)st.Attribute("Recursive"),
                        Filter = st.Attribute("Filter").Value,
                        ExcludedSubdirectories = st.Attribute("ExcludedSubdirectories")?.Value ?? string.Empty,
                        LastSyncDate = ((DateTime)st.Attribute("LastSynced")).Equals(new DateTime()) ? (DateTime?)null : (DateTime)st.Attribute("LastSynced")
                    })
                 {
                     SyncModeType = (SyncModeType)Enum.Parse(typeof(SyncModeType), st.Attribute("SyncMode").Value)
                 }),
                t.Attribute("Name").Value)
            {
                LastSyncDate = ((DateTime)t.Attribute("LastSynced")).Equals(new DateTime()) ? (DateTime?)null : (DateTime)t.Attribute("LastSynced")
            });
        }

        /// <summary>
        /// XML serialization.
        /// </summary>
        /// <param name="tasklists">The tasklists.</param>
        /// <returns>The serialized XML.</returns>
        public static XDocument ToXml(this IEnumerable<TasklistViewModel> tasklists)
        {
            var doc = new XDocument(new XElement("Tasks"));

            foreach (var tasklist in tasklists)
            {
                doc.Root.Add(tasklist.ToXml());
            }

            return doc;
        }

        /// <summary>
        /// Converts a <see cref="TasklistViewModel"/> to XML.
        /// </summary>
        /// <param name="tasklist">The tasklist.</param>
        /// <returns>The serialized XML.</returns>
        private static XElement ToXml(this TasklistViewModel tasklist)
        {
            var element = new XElement(
                "Task",
                new XAttribute("Name", tasklist.Name),
                new XAttribute("LastSynced", tasklist.LastSyncDate ?? new DateTime()));

            foreach (var syncTask in tasklist)
            {
                element.Add(syncTask.ToXml());
            }

            return element;
        }

        /// <summary>
        /// Converts a <see cref="SyncTaskViewModel"/> to XML.
        /// </summary>
        /// <param name="syncTask">The sync task.</param>
        /// <returns>The serialized XML.</returns>
        private static XElement ToXml(this SyncTaskViewModel syncTask)
        {
            return new XElement(
                "TaskItem",
                new XAttribute("Name", syncTask.Name),
                new XAttribute("Active", syncTask.IsActive),
                new XAttribute("SyncMode", syncTask.SyncModeType),
                new XAttribute("ReferenceDirectory", syncTask.ReferenceDirectory),
                new XAttribute("TargetDirectory", syncTask.TargetDirectory),
                new XAttribute("Recursive", syncTask.Recursive),
                new XAttribute("Filter", syncTask.Filter),
                new XAttribute("ExcludedSubdirectories", syncTask.ExcludedSubdirectories),
                new XAttribute("LastSynced", syncTask.LastSyncDate ?? new DateTime()));
        }
    }
}
