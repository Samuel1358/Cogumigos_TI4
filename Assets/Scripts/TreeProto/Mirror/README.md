# Sistema de Puzzle de Espelhos e Feixe de Luz - Sistema de Etapas

Este sistema permite criar puzzles sequenciais onde o jogador deve completar etapas usando feixes de luz e espelhos. Cada etapa é completada ao atingir um alvo que desabilita um objeto, permitindo interação com uma alavanca que avança para a próxima etapa.

## Componentes do Sistema

### 1. LightBeamEmitter
**Descrição:** Emite um feixe de luz usando LineRenderer quando ativado por um ActivateSwitch.

**Como usar:**
1. Adicione o componente a um GameObject
2. Configure as referências:
   - `Beam Start Point`: Ponto de onde o feixe sai (opcional, usa o próprio transform se não definido)
   - `Activate Switch`: Referência ao ActivateSwitch que controlará o emissor
3. Configure as camadas:
   - `Reflectable Layers`: Camadas que podem refletir o feixe (espelhos)
   - `Obstacle Layers`: Camadas que bloqueiam o feixe
4. Configure os visuais:
   - `Beam Material`: Material para o LineRenderer
   - `Beam Width`: Espessura do feixe
   - `Beam Color`: Cor do feixe

### 2. MirrorReflector
**Descrição:** Reflete o feixe de luz com 4 direções possíveis e animação suave.

**Características:**
- **4 Direções**: Up (0°), Right (90°), Down (180°), Left (-90°)
- **Animação**: Rotação suave de 1 segundo entre direções
- **Interação**: Pode ser controlado via `MirrorInteraction` + `InteractingArea`
- **Estrutura Pai/Filho**: Funciona com objeto pai rotacionando e filho com MirrorReflector
- **Reflexão Inteligente**: Calcula reflexão baseada na rotação do pai automaticamente

**Como usar:**
1. Adicione o componente ao GameObject filho "Mirror"
2. Configure o espelho:
   - `Use Transform Forward`: Se true, usa a direção forward do transform como normal
   - `Mirror Normal`: Direção normal do espelho (se não usar transform forward)
   - `Reflection Direction`: Direção inicial (Up/Right/Down/Left)
3. Configure animação:
   - `Rotation Animation Duration`: Duração da animação (padrão: 1s)
   - `Rotation Curve`: Curva de animação suave
4. Configure efeitos visuais opcionais:
   - `Reflection Effect`: GameObject que aparece quando o feixe atinge o espelho
   - `Effect Duration`: Duração do efeito visual

### 2.1. MirrorInteraction + MirrorInteractionScript
**Descrição:** Sistema completo de interação com espelhos seguindo o padrão do projeto.

**Componentes:**
- **MirrorInteraction**: MonoBehaviour principal (vai no objeto base)
- **MirrorInteractionScript**: Classe Interaction (criada automaticamente)

**Características:**
- **Segue padrão existente**: Mesmo sistema que ActivateSwitch usa
- **Auto-detecção**: Encontra automaticamente o `MirrorReflector` nos filhos
- **Feedback sonoro**: Som opcional ao rotacionar
- **Prevenção de spam**: Impede interação durante animação
- **Context menus**: Testes rápidos no inspector

**Como usar:**
1. Adicione `MirrorInteraction` ao GameObject base/pai do espelho
2. Adicione `InteractingArea` ao mesmo objeto (com Collider como trigger)
3. Configure no `MirrorInteraction`:
   - `Area`: Referência para o InteractingArea
   - `Auto Find Mirror`: true (encontra automaticamente)
   - `Interact Just Once`: false (permite múltiplas interações)
   - `Rotation Sound`: Som opcional de rotação
4. **Estrutura recomendada:**
   ```
   Mirror Base (MirrorInteraction + InteractingArea + Collider)
   ├── Mirror (MirrorReflector script + Cubo fino como collider)
   └── Base (modelo visual)
   ```

**Importante para Reflexão:**
- O espelho está sempre na diagonal e **sempre reflete em exatamente 90°**
- A cada rotação do pai, **alterna entre +90° e -90°** (muda o lado da reflexão)
- Comportamento: 0° = +90°, 90° = -90°, 180° = +90°, 270° = -90°
- **Exemplo**: Feixe horizontal → sempre vira vertical, mas alterna entre ↑ e ↓
- Funciona com qualquer geometria (cube, plane, etc.) no filho
- O pai pode rotacionar livremente, a reflexão se adapta automaticamente

**Padrão de Reflexão:**
```
Rotação 0°:   Horizontal → Vertical (+90°)
Rotação 90°:  Horizontal → Vertical (-90°) 
Rotação 180°: Horizontal → Vertical (+90°)
Rotação 270°: Horizontal → Vertical (-90°)
```

**Nota técnica:** O sistema cria automaticamente uma instância de `MirrorInteractionScript` que herda de `Interaction`, seguindo o padrão de arquitetura do projeto.

### 3. LightBeamTarget
**Descrição:** Detecta quando o feixe de luz o atinge e executa ações.

**Como usar:**
1. Adicione o componente ao GameObject do alvo
2. Configure comportamento:
   - `Requires Continuous Hit`: Se true, precisa manter o feixe no alvo
   - `Activation Delay`: Delay antes de ativar o alvo
   - `One Time Activation`: Se true, só ativa uma vez
3. Configure feedback visual:
   - `Hit Effect`: Efeito que aparece quando é atingido
   - `Indicator Light`: Luz que muda de cor quando ativado
   - `Target Renderer`: Renderer que muda de cor
4. Configure eventos UnityEvent:
   - `onBeamHit`: Quando o feixe atinge
   - `onTargetActivated`: Quando o alvo é ativado
   - `onTargetDeactivated`: Quando o alvo é desativado

### 4. LightPuzzleManager
**Descrição:** Gerencia todo o puzzle com sistema de etapas sequenciais.

**Como usar:**
1. Adicione o componente a um GameObject na cena
2. Configure as etapas no array `Puzzle Stages`:
   - `Stage Name`: Nome da etapa
   - `Emitter`: Emissor de luz da etapa
   - `Mirrors`: Espelhos da etapa
   - `Target`: Alvo da etapa
   - `Lever Interaction`: Transform da alavanca (para etapas 1-2)
   - `Stage Objects`: Todos os objetos da etapa (para animação)
3. Configure transições:
   - `Sink Distance`: Distância para afundar objetos
   - `Rise Distance`: Distância para subir objetos
4. Configure eventos:
   - `onPuzzleCompleted`: Quando todo o puzzle é completado
   - `onStageCompleted`: Quando uma etapa é completada
   - `onStageStarted`: Quando uma etapa inicia



## Setup Passo a Passo - Sistema de Etapas

### 1. Configuração Básica
1. **Gerenciador**: Crie um GameObject vazio e adicione `LightPuzzleManager`
2. **Estrutura**: Organize objetos em hierarquia por etapas (Stage1, Stage2, Stage3)
3. **Posicionamento**: Posicione elementos das etapas 2-3 nas posições finais
4. **Configuração**: Configure `Hide Inactive Stages = true` para esconder etapas inativas

### 2. Para Cada Etapa (1-3):
1. **Emissor**: 
   - Crie GameObject com `ActivateSwitch` + `InteractingArea` + `LightBeamEmitter`
   - Configure material e cores do LineRenderer
   
2. **Espelhos**:
   - **Estrutura**: GameObject base → Mirror (filho) + Base (filho)
   - **Base**: Adicione `MirrorInteraction` + `InteractingArea` + Collider
   - **Mirror**: Adicione `MirrorReflector` com direção inicial (Up/Right/Down/Left)
   - **Configuração**: O jogador pode rotacionar via interação (tecla E)
   - **Animação**: Rotação suave de 1 segundo entre as 4 direções
   
3. **Alvo**:
   - Adicione `LightBeamTarget` com `Object To Disable` configurado
   - O objeto a desabilitar deve bloquear acesso à alavanca
   
4. **Alavanca** (apenas etapas 1-2):
   - Use `ActivateSwitch` + `InteractingArea` (sistema existente)
   - O `LightPuzzleManager` se conecta automaticamente ao evento `onActivate`
   - **Etapa 3**: Use teleporte ao invés de alavanca

### 3. Configuração do LightPuzzleManager
1. **Array Puzzle Stages**: Configure 3 etapas
2. **Para cada etapa**:
   - `Stage Name`: Nome descritivo
   - `Emitter`: Referência ao emissor
   - `Mirrors`: Array com espelhos da etapa
   - `Target`: Referência ao alvo
   - `Lever Switch`: ActivateSwitch da alavanca (ou TP para etapa 3)
   - `Stage Objects`: Todos os objetos que devem animar (emissor, espelhos, alvo, alavanca)
   - `Animation Duration`: Duração da transição (2s recomendado)

### 4. Configuração de Transições
- `Sink Distance`: 5 unidades (objetos afundam)
- `Rise Distance`: 5 unidades (objetos sobem)
- `Hide Inactive Stages`: true (esconde etapas 2-3 inicialmente)

## Layers Recomendadas

Crie as seguintes layers no projeto:
- `Mirror`: Para objetos espelho
- `LightTarget`: Para alvos do feixe
- `LightObstacle`: Para obstáculos que bloqueiam o feixe

## Dicas de Uso

1. **Debug Visual**: Os scripts têm debug gizmos que ajudam a visualizar no Scene View
2. **Teste no Inspector**: Use os métodos de contexto menu para testar funcionalidades
3. **LayerMask**: Configure corretamente as layers para evitar problemas de detecção
4. **Performance**: O sistema calcula o caminho do feixe a cada frame quando ativo
5. **Efeitos Visuais**: Adicione partículas e sons para melhor feedback
6. **Material do Feixe**: Use um material emissor ou com shader especial para o feixe

## Exemplo de Configuração

```
Emissor:
- Position: (0, 1, 0)
- Rotation: olhando para frente
- Max Beam Distance: 50
- Reflectable Layers: Mirror
- Obstacle Layers: Default, LightObstacle

Espelho 1:
- Position: (5, 1, 0)
- Rotation: 45 graus Y para refletir 90 graus
- Layer: Mirror
- Use Transform Forward: true

Alvo:
- Position: (5, 1, 5)
- Layer: LightTarget
- Requires Continuous Hit: false
```

## Troubleshooting

**Feixe não aparece:**
- Verifique se o ActivateSwitch está funcionando
- Verifique se o LineRenderer tem material
- Verifique se o emissor está ativo

**Reflexão não funciona:**
- Verifique se o espelho está na layer correta
- Verifique se o espelho tem MirrorReflector
- Verifique se o Collider do espelho está ativo

**Alvo não é atingido:**
- Verifique se o alvo tem Collider
- Verifique se está na layer correta
- Verifique se tem o componente LightBeamTarget 