# Drawer

## ConsoleExtension osztály

[Osztály kódjának megnyitása](./ConsoleDrawer2/ConsoleExtension.cs)

Extra beállításokat biztosít a Windows parancssorához.

### SetCodePage

Ki és bemeneti kódlapok beállítására, paraméterként meg kell adni a kódlap azonosítóját.

```csharp
ConsoleExtension.SetCodePage(65001u);²
```

### SetConsoleFont

Betûméret és típus beállítására.

```csharp
ConsoleExtension.SetConsoleFont("Consolas", 16);
```
