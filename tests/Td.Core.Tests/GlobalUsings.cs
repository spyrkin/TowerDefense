// ---------------------------------------------------------------------------
// Глобальные using тестового проекта Td.Core.Tests.
//
// ВАЖНО:
// Здесь нельзя добавлять global using Td.Core.* до тех пор,
// пока соответствующие пространства имён не получат хотя бы один тип
// в сборке Td.Core.
//
// Пустые namespace, объявленные в src/Td.Core/Namespaces.cs,
// не попадают в метаданные Td.Core.dll, поэтому внешний проект
// Td.Core.Tests не может на них ссылаться через using.
//
// Каждое пространство имён добавляется сюда поэтапно:
//
// R13  global using Td.Core.Domain.Common;   — DONE
// R14  global using Td.Core.Domain.Game;     — DONE
// R15  global using Td.Core.Domain.Maps;
// R16  global using Td.Core.Domain.Monsters;
// R17  global using Td.Core.Domain.Towers;
// R18  global using Td.Core.Domain.Waves;
// R19  global using Td.Core.Content;
// R20  global using Td.Core.Services.Audio;
// R21  global using Td.Core.Services.Settings;
// R22  global using Td.Core.Simulation;
//
// Базовые using System.* подключаются через ImplicitUsings.
// Xunit подключается через <Using Include="Xunit" /> в csproj.
// ---------------------------------------------------------------------------

global using Td.Core.Domain.Common;
global using Td.Core.Domain.Game;