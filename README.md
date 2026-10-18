# WarOrgan.Data

The data model and tools for building datasets for WarOrgan, with an example dataset to help you get started.
We recommend forking this repository to create and maintain your own dataset. Keep your fork updated with upstream changes to receive updates to the shared model and build tools.

## What's included

- **WarOrgan.Data.Model** — the data model shared with the WarOrgan app.
- **WarOrgan.Data.BuildDataset** — a console tool for loading, validating and packaging dataset JSON files.
- **Example dataset** — five faction rosters and a dataset definition showing how to use the model. See the [example notes and attribution](WarOrgan.Data.BuildDataset/Assets/Fracture/README.md).

## Building a dataset

You need the .NET 10 SDK.
From the repository root, build the included example with:

```powershell
dotnet run --project WarOrgan.Data.BuildDataset
```

The builder validates the example files and writes a dataset ZIP archive to the executable's `Output` directory. It logs the full output path when the build succeeds. For the included example, the archive is named `Fracture.zip`.

### Loading the result in WarOrgan

1. Open WarOrgan 3.0 or later and go to **Data Sets**.
2. Select **Import from file** and choose the generated ZIP archive. Import the ZIP directly; do not extract it first.
3. It should now be available as a selectable dataset, armylists on the home page are tied to the selected dataset.

## Creating your own dataset

1. Fork this repository, then clone your fork locally.
2. Start with the [minimal worked example](#minimal-worked-example) below, or use the JSON files in the [Fracture example folder](WarOrgan.Data.BuildDataset/Assets/Fracture) for more detailed examples.
3. Add your own asset folder, including a `README.md`, and update `defaultDatasetId`, `defaultVersion` and `assetsFolder` in your copy of `Program.cs`.
4. Run the builder, resolve any validation errors, and import the generated ZIP into WarOrgan using the steps above.

For compatibility with the existing WarOrgan app, adapt your data and build process to the supplied model. You are free to modify the model in your own project under the MIT licence, but those changes may require corresponding changes to the app.

### Dataset IDs and versions

- Choose a dataset ID that distinguishes your dataset from other datasets. Keep it unchanged when publishing updates to the same dataset.
- Existing army lists store this ID in `CreatedWithDataSetId`. They require a compatible dataset with a matching ID; changing the dataset ID creates a different dataset rather than updating the existing one.
- Increase `defaultVersion` whenever you publish an updated dataset, for example from `new Version(1, 1, 0, 0)` to `new Version(1, 1, 1, 0)`. 
- The version is written to the archive's `DataSetInfo.json`.
- The app only uses the "minor" and "build" properties of Version so I'd recommend using the "build" property for data changes and "minor" for when there is a format change.
- `DataSetFormatVersion` describes the shared file format. Leave it set to `DataSetInfo.CurrentDataSetFormatVersion`; use `defaultVersion` for your own dataset releases.
- Keep unit IDs stable where possible, because saved lists also refer to units by ID. Use `DisplayName` to change the visible name without changing the ID.

### Minimal worked example

This example defines one faction containing one model with one weapon. Create the following files under `WarOrgan.Data.BuildDataset/Assets/MyGame/`:

```text
MyGame/
  README.md
  DataSetDefinition.json
  Rosters/
    Example Faction.json
```

In `README.md`, add a short description of your dataset and any required attribution. The builder includes this file in the archive.

Use this for `DataSetDefinition.json`:

```json
{
  "Features": "None",
  "Factions": [
    {
      "Name": "Example Faction",
      "FactionKeyword": "Example",
      "MainColor": "#315C91",
      "DarkColor": "#6FA4D9"
    }
  ],
  "BattleSizeDefinitions": [
    {
      "Name": "Skirmish",
      "PointsTotal": 500,
      "DetachmentPoints": 0,
      "EnhancementLimit": 0,
      "UnitLimit": 0
    }
  ]
}
```

Use this for `Rosters/Example Faction.json`:

```json
{
  "Name": "Example Faction",
  "Units": [
    {
      "Id": "EXAMPLE TROOPER",
      "DisplayName": "Example Trooper",
      "FactionKeywords": ["Example"],
      "UnitComposition": {
        "ModelCompositions": [
          {
            "ModelName": "Trooper",
            "Limit": { "Min": 1, "Max": 1 },
            "StatlineRef": "Trooper",
            "Wargear": [
              { "Models": 1, "InitialWargear": ["rifle"] }
            ]
          }
        ]
      },
      "Points": [
        { "ModelCount": 1, "Cost": 10 }
      ],
      "StatLines": [
        {
          "StatName": "Trooper",
          "Stats": [
            { "Id": "Move", "Value": "6" },
            { "Id": "Defence", "Value": "4+" }
          ]
        }
      ],
      "Weapons": [
        {
          "Id": "rifle",
          "Weapons": [
            {
              "Id": "rifle",
              "Stats": [
                { "Id": "Range", "Value": "24" },
                { "Id": "Attacks", "Value": "1" }
              ]
            }
          ]
        }
      ]
    }
  ]
}
```

The important connections are:

| Field or file | Meaning |
| --- | --- |
| Faction `Name`, roster `Name`, and `Example Faction.json` | These must match exactly, with `.json` added for the filename. |
| Faction `FactionKeyword` and unit `FactionKeywords` | A unit must contain a keyword recognised by its faction. Here both use `Example`. |
| Unit `Id` and `DisplayName` | `Id` is the stable reference; `DisplayName` is the visible label. Give units distinct IDs within their roster. |
| `StatlineRef` and `StatName` | The model references its statline by name. Here both use `Trooper`. |
| `InitialWargear` and weapon `Id` | Starting equipment refers to weapon IDs. Here `rifle` selects the weapon profile inside the weapon set. |
| Model `Limit` and points `ModelCount` | The unit contains exactly one model, with a matching one-model points cost of 10. |
| `Features` | Enables optional app sections such as army rules and detachments. This example uses `None`; see [DataSetFeatures](WarOrgan.Data.Model/DataSet/DataSetFeatures.cs) for the available flags. |

In `Program.cs`, set `defaultDatasetId` to `"MyGame"`, set `defaultVersion` to `new Version(1, 1, 0, 0)`, and change `assetsFolder` to:

```csharp
var assetsFolder = Path.Combine(AppContext.BaseDirectory, "Assets", "MyGame").EnsureDirectoryExists();
```

Run `dotnet run --project WarOrgan.Data.BuildDataset` from the repository root. The builder should report one roster and one unit, and write `MyGame.zip` to its `Output` directory. Import that ZIP into WarOrgan to try it.

## Model compatibility and change requests

`WarOrgan.Data.Model` is tied to the WarOrgan app. Changes to the shared model can affect whether the app can read a dataset, so changes accepted into this repository must be agreed and coordinated with the app.

Keep the model unchanged when creating compatible datasets. If the current model cannot represent something you need, open an issue in this repository or email [warorgan@gmail.com](mailto:warorgan@gmail.com) to discuss it before submitting a model change.

Improvements to the build tools, validation and documentation are welcome. Please keep your game datasets in your own repository.

The model may change over time. If it does, I'll do my best to support existing datasets through migration tools or backwards compatibility in the app.

## Licence

The model, build tools and project documentation are available under the [MIT License](LICENSE). You are free to use, modify and share them under its terms. They are provided as is, without warranty, with the liability disclaimer set out in the licence.
