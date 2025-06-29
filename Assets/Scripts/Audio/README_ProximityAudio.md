# Sistema de Áudio por Proximidade

## Visão Geral

O sistema de áudio por proximidade permite que efeitos sonoros sejam tocados apenas quando o player está próximo, criando uma experiência de áudio mais imersiva e otimizada.

## Como Funciona

### Configuração Básica

1. **Configure o AudioManager**:
   - `Proximity Distance`: Distância máxima para tocar sons (padrão: 5 unidades)
   - `Show Proximity Debug`: Ativa logs de debug para acompanhar o sistema

2. **Configure o Player**:
   - Certifique-se de que o player tem a tag "Player"
   - O sistema automaticamente encontra o player pela tag

### Métodos Disponíveis

#### 1. `PlaySFXIfPlayerNearby()`
```csharp
bool soundPlayed = AudioManager.Instance.PlaySFXIfPlayerNearby(
    "SoundName",           // Nome do efeito sonoro
    transform.position,    // Posição onde tocar o som
    1f,                    // Volume (0-1)
    1f                     // Pitch (0-1)
);

if (soundPlayed)
{
    Debug.Log("Som tocado - player estava próximo!");
}
else
{
    Debug.Log("Som não tocado - player muito longe");
}
```

#### 2. `IsPlayerNearby()`
```csharp
bool nearby = AudioManager.Instance.IsPlayerNearby(transform.position);
if (nearby)
{
    // Player está próximo, pode fazer algo
}
```

#### 3. `SetPlayerTransform()`
```csharp
// Configurar manualmente o player (opcional)
AudioManager.Instance.SetPlayerTransform(playerTransform);
```

## Exemplos de Uso

### Exemplo 1: Som ao Passar por um Trigger
```csharp
private void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("Player"))
    {
        AudioManager.Instance.PlaySFXIfPlayerNearby("Footstep", transform.position);
    }
}
```

### Exemplo 2: Som ao Interagir
```csharp
private void Update()
{
    if (Input.GetKeyDown(KeyCode.E))
    {
        AudioManager.Instance.PlaySFXIfPlayerNearby("Interact", transform.position);
    }
}
```

### Exemplo 3: Som Condicional
```csharp
private void PlaySoundIfNearby()
{
    if (AudioManager.Instance.IsPlayerNearby(transform.position))
    {
        // Player está próximo, tocar som
        AudioManager.Instance.PlaySFX("AmbientSound");
    }
}
```

## Script de Exemplo: ProximityAudioExample

O script `ProximityAudioExample` demonstra diferentes formas de usar o sistema:

### Configurações Disponíveis

- **Audio Settings**:
  - `Sound Effect Name`: Nome do efeito sonoro
  - `Volume`: Volume do som (0-1)
  - `Pitch`: Pitch do som (0-1)

- **Trigger Settings**:
  - `Play On Trigger`: Toca quando player entra no trigger
  - `Play On Collision`: Toca quando player colide
  - `Play On Interaction`: Toca quando player pressiona E
  - `Interaction Distance`: Distância para interação

- **Debug**:
  - `Show Debug Info`: Mostra logs de debug

### Como Usar o Script

1. **Adicione o script a um GameObject**
2. **Configure o som desejado** no campo "Sound Effect Name"
3. **Escolha o tipo de trigger** (trigger, collision, interaction)
4. **Ajuste a distância** se necessário

### Visualização no Scene View

- **Verde**: Player está próximo (som será tocado)
- **Vermelho**: Player está longe (som não será tocado)
- **Azul**: Área de interação (se `Play On Interaction` estiver ativo)
- **Amarelo**: Área de proximidade do AudioManager

## Configuração no Inspector

### AudioManager Settings
```
Proximity Distance: 5.0    // Distância máxima para tocar sons
Show Proximity Debug: false // Logs de debug
```

### ProximityAudioExample Settings
```
Sound Effect Name: "Footstep"
Volume: 1.0
Pitch: 1.0
Play On Trigger: true
Play On Collision: false
Play On Interaction: false
Show Debug Info: false
```

## Dicas de Uso

### 1. **Otimização de Performance**
- Use `IsPlayerNearby()` para verificações frequentes
- Use `PlaySFXIfPlayerNearby()` apenas quando necessário

### 2. **Configuração de Distâncias**
- Sons ambientes: 8-12 unidades
- Sons de interação: 2-4 unidades
- Sons de passos: 3-5 unidades

### 3. **Debug e Teste**
- Ative `Show Proximity Debug` para ver logs
- Use os gizmos no Scene View para visualizar distâncias
- Teste diferentes distâncias para encontrar o ideal

### 4. **Integração com Outros Sistemas**
```csharp
// Exemplo: Integração com sistema de interação
public class InteractableObject : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (AudioManager.Instance.IsPlayerNearby(transform.position))
        {
            AudioManager.Instance.PlaySFXIfPlayerNearby("Click", transform.position);
            // Executar ação de interação
        }
    }
}
```

## Troubleshooting

### Problema: "Player not found! Cannot check proximity"
**Solução**: Certifique-se de que o player tem a tag "Player"

### Problema: Sons não tocam
**Solução**: 
1. Verifique se o nome do som está correto
2. Verifique se a distância está adequada
3. Ative debug para ver logs

### Problema: Sons tocam muito longe
**Solução**: Reduza o `Proximity Distance` no AudioManager

### Problema: Performance ruim
**Solução**: 
1. Use `IsPlayerNearby()` para verificações frequentes
2. Reduza a frequência de verificações
3. Use cooldowns para evitar spam

## Exemplo Completo

```csharp
public class AmbientSoundController : MonoBehaviour
{
    [SerializeField] private string soundName = "Ambient";
    [SerializeField] private float checkInterval = 1f;
    
    private float lastCheckTime = 0f;
    
    private void Update()
    {
        if (Time.time - lastCheckTime > checkInterval)
        {
            if (AudioManager.Instance.IsPlayerNearby(transform.position))
            {
                AudioManager.Instance.PlaySFXIfPlayerNearby(soundName, transform.position);
            }
            lastCheckTime = Time.time;
        }
    }
}
```

Este sistema torna o áudio mais imersivo e otimizado, tocando sons apenas quando fazem sentido para o player! 🎵 