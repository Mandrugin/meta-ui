# Architecture

```plantuml
@startuml
[Entities]<--[Gateways]
[Entities]<--[UseCases]
[UseCases]<|-[Gateways]
[Gateways]->[DataConfigs]
[UseCases]<--[Presenters]
[Presenters]<--[Views]
[Views]->[ViewConfigs]
@enduml
```
