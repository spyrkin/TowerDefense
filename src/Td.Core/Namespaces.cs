// ---------------------------------------------------------------------------
// Каркасный файл для безопасной миграции на целевые пространства имён (шаг R10).
//
// Файл объявляет пустые пространства имён, чтобы:
//   1. global using в проекте и тестах (шаги R11, R12) компилировались
//      до фактического переноса типов;
//   2. шаги R13-R22 могли переносить типы по одному модулю,
//      не ломая сборку на каждом файле.
//
// Файл не содержит типов и не влияет на поведение программы.
// После завершения миграции всех модулей (R13-R22) файл может быть удалён,
// так как все пространства имён будут объявлены в реальных файлах.
// ---------------------------------------------------------------------------

namespace Td.Core.Domain.Common
{
}

namespace Td.Core.Domain.Game
{
}

namespace Td.Core.Domain.Maps
{
}

namespace Td.Core.Domain.Monsters
{
}

namespace Td.Core.Domain.Towers
{
}

namespace Td.Core.Domain.Waves
{
}

namespace Td.Core.Content
{
}

namespace Td.Core.Services.Audio
{
}

namespace Td.Core.Services.Settings
{
}

namespace Td.Core.Simulation
{
}