﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace NoteApp
{
    /// <summary>
    /// <para>Класс "Менеджер проекта".</para>
    /// <para>Реализует метод для сохранения объекта класса <see cref="Project"/> 
    /// в файл и метод загрузки объекта <see cref="Project"/> из файла.</para>
    /// </summary>
    public static class ProjectManager
    {
        /// <summary>
        /// Имя файла, в который осуществляется сохранение и из которого производится загрузка 
        /// объекта класса <see cref="Project"/>.
        /// </summary>
        private const string Filename = "NoteApp.notes";

        /// <summary>
        /// Возвращает и задает путь по умолчанию к файлу, имя которого задано закрытой константой 
        /// <see cref="Filename"/>. 
        /// </summary>
        public static string DefaultPath { get; set; } = 
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), 
                "NoteApp", Filename);

        /// <summary>
        /// Метод, осуществляющий сохранение объекта класса <see cref="Project"/> в файл, имя 
        /// которого задано закрытой константой <see cref="Filename"/>.
        /// </summary>
        /// <param name="data">Объект класса "Проект", который требуется сериализовать.</param>
        /// <param name="path"><para>Путь к файлу, в который осуществляется сохранение данных.</para> 
        /// <para>Путь по умолчанию указан в <see cref="DefaultPath"/>.</para></param>
        public static void SaveToFile(Project data, string path)
        {
            // Если какой-либо каталог (и/или подкаталоги), указанные в пути к файлу, в который
            // необходимо сохранить данные, не существует, то предварительно создаем этот каталог
            // (и/или подкаталоги).
            //
            if(!Directory.Exists(Path.GetDirectoryName(path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }

            JsonSerializer serializer = new JsonSerializer();

            using (StreamWriter sw = new StreamWriter(path))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                // Настраиваем форматирование в итоговом файле (автоматическую табуляцию вложенных
                // типов) для лучшего восприятия текста.
                serializer.Formatting = Formatting.Indented;

                serializer.Serialize(writer, data);
            }
        }

        /// <summary>
        /// Метод, осуществляющий загрузку данных из файла, имя которого задано закрытой константой 
        /// <see cref="Filename"/>, в объект класса <see cref="Project"/>.
        /// </summary>
        /// <param name="path"><para>Путь к файлу, из которого осуществляется загрузка данных.</para> 
        /// <para>Путь по умолчанию указан в <see cref="DefaultPath"/>.</para></param>
        /// <returns>Объект класса <see cref="Project"/>, в который помещен результат десериализации.</returns>
        public static Project LoadFromFile(string path)
        {
            Project project = null;

            // Если файл, из которого необходимо загрузить данные, не существует, то возвращаем объект
            // класса Project, созданный конструктором по умолчанию.
            //
            if (!File.Exists(path))
            {
                return new Project();
            }

            JsonSerializer serializer = new JsonSerializer();

            // В случае возникновения любых исключений в процессе открытия потока для чтения из файла
            // также возвращаем объект класса Project, созданный конструктором по умолчанию.
            //
            try
            {
                using (StreamReader sr = new StreamReader(path))
                using (JsonReader reader = new JsonTextReader(sr))
                {
                    project = serializer.Deserialize<Project>(reader);
                }
            }
            catch (Exception)
            {
                return new Project();
            }

            // Метод в любом случае должен возвращать объект класса Project.
            return project ?? new Project();
        }
    }
}
