# LillypadSpawner - Sistema Dinâmico de Waypoints

## Visão Geral

O `LillypadSpawner` foi refatorado para permitir uma quantidade dinâmica de waypoints em diferentes fases, corrigindo o problema de adjacência circular que causava movimentos estranhos das lillypads.

## Principais Melhorias

### 1. **Quantidade Dinâmica de Waypoints**
- Agora você pode ter diferentes quantidades de waypoints em cada fase
- Suporte para 2, 3, 4 ou mais fases
- Cada fase pode ter qualquer número de waypoints

### 2. **Adjacência Corrigida**
- **Antes**: O sistema considerava o primeiro waypoint como adjacente ao último (comportamento circular)
- **Agora**: Apenas waypoints realmente próximos são considerados adjacentes
- Controle de distância máxima de adjacência por fase

### 3. **Sistema de Fases Flexível**
- Cada fase tem seu próprio nome e configuração
- Controle individual da distância máxima de adjacência
- Validação automática de configuração

### 4. **Evita Repetição de Waypoints** ⭐ **NOVO**
- **Problema anterior**: Waypoints podiam ser repetidos entre lillypads consecutivas
- **Solução**: Sistema rastreia o último waypoint usado em cada fase
- **Comportamento**: A próxima lillypad sempre escolhe um waypoint diferente do usado anteriormente
- **Fallback**: Se não houver opções disponíveis, permite repetição para evitar travamentos

## Como Usar

### Configuração Básica no Inspector

1. **Adicione o componente `LillypadSpawner` ao seu GameObject**
2. **Configure as fases de waypoints**:
   - `Waypoint Phases`: Array de fases (padrão: 3 fases)
   - Cada fase tem:
     - `Phase Name`: Nome da fase (ex: "Start", "Middle", "End")
     - `Waypoints`: Array de Transform dos waypoints
     - `Max Adjacent Distance`: Distância máxima para considerar waypoints adjacentes (1-3)

### Exemplo de Configuração

```csharp
// Fase 1 (Início): 4 waypoints
Phase Name: "Start"
Waypoints: [StartPoint1, StartPoint2, StartPoint3, StartPoint4]
Max Adjacent Distance: 1

// Fase 2 (Meio): 6 waypoints  
Phase Name: "Middle"
Waypoints: [MiddlePoint1, MiddlePoint2, MiddlePoint3, MiddlePoint4, MiddlePoint5, MiddlePoint6]
Max Adjacent Distance: 2

// Fase 3 (Fim): 3 waypoints
Phase Name: "End"
Waypoints: [EndPoint1, EndPoint2, EndPoint3]
Max Adjacent Distance: 1
```

### Configuração Programática

Use o script `LillypadSpawnerExample` para configurar diferentes cenários:

```csharp
// Exemplo 1: Configuração simples 3 fases
spawner.AddWaypointPhase("Start", startWaypoints, 1);
spawner.AddWaypointPhase("Middle", middleWaypoints, 1);
spawner.AddWaypointPhase("End", endWaypoints, 1);

// Exemplo 2: Mais waypoints no meio
spawner.AddWaypointPhase("Start", startWaypoints, 1);
spawner.AddWaypointPhase("Middle", manyMiddleWaypoints, 2); // Distância 2
spawner.AddWaypointPhase("End", endWaypoints, 1);

// Exemplo 3: Apenas 2 fases
spawner.AddWaypointPhase("Start", startWaypoints, 1);
spawner.AddWaypointPhase("End", endWaypoints, 1);
```

## Lógica de Adjacência

### Como Funciona

1. **Posição Atual**: A lillypad está no waypoint `i` da fase atual
2. **Cálculo de Adjacentes**: Para a próxima fase, considera waypoints de `i-maxDistance` até `i+maxDistance`
3. **Limites**: Apenas waypoints dentro dos limites do array são considerados (sem wrapping circular)

### Exemplo Prático

```
Fase Atual (índice 2): [A, B, C, D, E]
Próxima Fase (índice 3): [X, Y, Z, W]

Se a lillypad está no waypoint C (índice 2) e maxDistance = 1:
- Adjacentes válidos: Y (índice 1), Z (índice 2), W (índice 3)
- X (índice 0) NÃO é considerado adjacente (muito longe)
```

## Sistema Anti-Repetição ⭐ **NOVO**

### Como Funciona

1. **Rastreamento**: O sistema mantém um array `lastUsedWaypointIndices` que rastreia o último waypoint usado em cada fase
2. **Filtragem**: Ao gerar um novo caminho, o sistema filtra o último waypoint usado de cada fase
3. **Seleção**: Escolhe aleatoriamente entre os waypoints disponíveis (excluindo o último usado)
4. **Fallback**: Se não houver opções disponíveis, permite repetição para evitar travamentos

### Exemplo de Comportamento

```
Lillypad 1: Start[0] → Middle[2] → End[1]
Lillypad 2: Start[1] → Middle[3] → End[0]  // Diferentes waypoints
Lillypad 3: Start[2] → Middle[1] → End[2]  // Diferentes waypoints
```

### Controle Programático

```csharp
// Resetar o rastreamento (útil para mudanças de nível)
spawner.ResetWaypointTracking();

// Verificar último waypoint usado em uma fase
int lastUsed = spawner.GetLastUsedWaypointIndex(1); // Fase 1 (Middle)
```

## Debug e Visualização

### Gizmos no Scene View

- **Verde**: Waypoints da primeira fase (início)
- **Azul**: Waypoints da segunda fase (meio)  
- **Vermelho**: Waypoints da terceira fase (fim)
- **Amarelo**: Conexões entre fases adjacentes

### Logs de Debug

Ative `Show Debug Info` para ver:
- Validação de configuração
- Número de waypoints por fase
- Caminhos gerados para cada lillypad
- **Novo**: Filtragem de waypoints repetidos
- **Novo**: Atualização do rastreamento de waypoints usados

## Validação Automática

O sistema valida automaticamente:
- ✅ Fases configuradas
- ✅ Waypoints não nulos
- ✅ Pelo menos uma fase válida
- ⚠️ Avisos para configurações inválidas

## Migração do Sistema Anterior

### Antes (Sistema Fixo)
```csharp
[SerializeField] private Transform[] spawnPoints = new Transform[4];
[SerializeField] private Transform[] middlePoints = new Transform[4];
[SerializeField] private Transform[] finalPoints = new Transform[4];
```

### Depois (Sistema Dinâmico)
```csharp
[SerializeField] private WaypointPhase[] waypointPhases = new WaypointPhase[3];
```

### Para Migrar:
1. Configure as 3 fases no Inspector
2. Mova os waypoints antigos para as fases correspondentes
3. Ajuste a distância de adjacência conforme necessário

## Dicas de Uso

1. **Para movimentos mais previsíveis**: Use `Max Adjacent Distance = 1`
2. **Para mais variedade**: Use `Max Adjacent Distance = 2` ou `3`
3. **Para fases simples**: Use apenas 2 fases (início e fim)
4. **Para movimentos complexos**: Use 4+ fases com diferentes quantidades de waypoints
5. **Para evitar repetições**: O sistema já faz isso automaticamente! ⭐
6. **Para resetar repetições**: Use `ResetWaypointTracking()` entre níveis

## Troubleshooting

### Problema: "No waypoint phases configured"
**Solução**: Configure pelo menos uma fase no Inspector

### Problema: "First phase has no waypoints"
**Solução**: Adicione waypoints à primeira fase

### Problema: Movimentos estranhos
**Solução**: Reduza `Max Adjacent Distance` para 1

### Problema: Pouca variedade de caminhos
**Solução**: Aumente `Max Adjacent Distance` ou adicione mais waypoints

### Problema: Waypoints se repetindo muito
**Solução**: Adicione mais waypoints às fases ou use `ResetWaypointTracking()` para limpar o histórico 