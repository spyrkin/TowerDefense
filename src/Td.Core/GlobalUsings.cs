// ---------------------------------------------------------------------------
// Глобальные using проекта Td.Core (шаг R11).
//
// Файл открывает всем файлам проекта доступ к целевым пространствам имён
// без необходимости добавлять using в каждый файл. Это позволяет шагам
// R13-R22 переносить типы в пространства имён, не ломая компиляцию
// существующих файлов.
//
// Базовые using пространства System здесь не объявляются,
// так как в проекте включён <ImplicitUsings>enable</ImplicitUsings>.
// ---------------------------------------------------------------------------

global using Td.Core.Domain.Common;
global using Td.Core.Domain.Game;
global using Td.Core.Domain.Maps;
global using Td.Core.Domain.Monsters;
global using Td.Core.Domain.Towers;
global using Td.Core.Domain.Waves;
global using Td.Core.Content;
global using Td.Core.Services.Audio;
global using Td.Core.Services.Settings;
global using Td.Core.Simulation;