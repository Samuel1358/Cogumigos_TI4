# 🎨 Sistema de Volumes de Pós-Processamento

Este sistema permite criar diferentes ambientações visuais em diferentes áreas da cena utilizando volumes de pós-processamento do Unity URP.

## 📋 **Pré-requisitos**

1. **Unity com URP (Universal Render Pipeline)** configurado
2. **Post-processing** habilitado na câmera principal
3. Layer configurada para o **Player** (para detecção de área)

## 🚀 **Configuração Inicial**

### 1. **Configurar o Post-Processing Manager**

1. Crie um GameObject vazio na cena chamado `PostProcessing Manager`
2. Adicione o script `PostProcessingManager.cs`
3. Configure os parâmetros no inspector:
   - **Main Camera**: Arraste sua câmera principal
   - **Auto Create Global Volume**: Deixe marcado
   - **Enable Anti Aliasing**: Recomendado
   - **Show Debug Info**: Para ver logs de configuração

### 2. **Verificar Configuração da Câmera**

A câmera deve ter:
- **Post Processing** habilitado (URP)
- **HDR** habilitado (recomendado)
- **Depth Texture** habilitado

## 🌟 **Criando Áreas com Efeitos**

### **Método 1: Configuração Automática (Recomendado)**

1. **Criar GameObject da Área**:
   - GameObject vazio: `Dark Forest Area`
   - Adicione um **BoxCollider** (marque como Trigger)
   - Posicione e redimensione para cobrir a área desejada

2. **Adicionar o Script**:
   - Adicione `PostProcessingVolumeController.cs`
   - Configure no inspector:
     - **Area Name**: "Floresta Sombria"
     - **Area Type**: Dark
     - **Auto Configure Effects**: ✓ marcado
     - **Player Layer**: Selecione a layer do seu player
     - **Transition Duration**: 1.0 (1 segundo de transição)

### **Método 2: Configuração Manual**

1. Crie a área como no Método 1
2. Crie um **Volume Profile** personalizado:
   - `Assets > Create > Rendering > URP Volume Profile`
   - Configure efeitos manualmente no Volume Profile
   - Atribua ao campo **Volume Profile** do script

## 🎭 **Tipos de Áreas Pré-configuradas**

### **🌑 Dark (Área Sombria/Ameaçadora)**
- **Vinheta**: Escura e densa (60% intensidade)
- **Color Adjustments**: Dessaturado (-30%), escuro (-0.5 exposure)
- **Contraste**: Aumentado (+10%)
- **Uso**: Florestas sombrias, masmorras, áreas perigosas

### **☀️ Bright (Área Alegre/Vibrante)**
- **Bloom**: Suave (30% intensidade)
- **Vinheta**: Clara e dourada (20% intensidade)
- **Color Adjustments**: Saturado (+15%), brilhante (+0.2 exposure)
- **Uso**: Clareiras ensolaradas, jardins, áreas seguras

### **🏔️ Underground (Subterrâneo)**
- **Color Adjustments**: Tons frios (-20° hue shift), dessaturado
- **Exposure**: Reduzida (-0.3)
- **Uso**: Cavernas, túneis, áreas subterrâneas

### **✨ Magical (Área Mágica)**
- **Bloom**: Intenso (50% intensidade)
- **Color Adjustments**: Tons roxos (+30° hue shift), saturado (+25%)
- **Uso**: Áreas mágicas, cristais, portais

### **☢️ Toxic (Área Tóxica)**
- **Color Adjustments**: Tons verdes (+60° hue shift)
- **Vinheta**: Esverdeada
- **Uso**: Pântanos, áreas contaminadas, laboratórios

## 🎮 **Exemplo Prático: Floresta com Duas Áreas**

### **Setup da Cena**:

```
Cena Floresta
├── PostProcessing Manager (PostProcessingManager.cs)
├── Main Camera (configurada com URP + Post-Processing)
├── Player (com layer "Player")
├── Floresta Normal (terreno base)
├── Área Sombria
│   ├── Dark Forest Volume (PostProcessingVolumeController.cs)
│   ├── BoxCollider (Trigger, cobrem a área escura)
│   └── Props visuais (árvores mortas, névoa)
└── Área Alegre
    ├── Bright Clearing Volume (PostProcessingVolumeController.cs)
    ├── BoxCollider (Trigger, cobre a clareira)
    └── Props visuais (flores, luz solar)
```

### **Configuração da Área Sombria**:
```csharp
// No inspector do PostProcessingVolumeController
Area Type: Dark
Area Name: "Floresta Sombria"
Transition Duration: 2.0f
Is Exclusive Area: false
Auto Configure Effects: ✓
Show Debug Logs: ✓
```

### **Configuração da Área Alegre**:
```csharp
// No inspector do PostProcessingVolumeController
Area Type: Bright
Area Name: "Clareira Ensolarada"
Transition Duration: 1.5f
Is Exclusive Area: false
Auto Configure Effects: ✓
Show Debug Logs: ✓
```

## 🔧 **Configurações Avançadas**

### **Áudio Ambiente Integrado**
- Adicione um **AudioSource** ao GameObject da área
- Configure o audio clip de ambiente
- O script automaticamente fará fade in/out do áudio

### **Áreas Exclusivas**
- Marque `Is Exclusive Area = true`
- Quando o player entrar, outras áreas serão desativadas
- Útil para efeitos dramáticos ou mudanças bruscas

### **Prioridades de Volume**
- Volumes locais têm prioridade sobre o global
- Use `Volume.priority` para controlar sobreposição
- Valores maiores = maior prioridade

### **Transições Customizadas**
- Ajuste `Transition Duration` para controlar velocidade
- Use curvas personalizadas modificando o código
- Adicione easing functions para transições suaves

## 🐛 **Debug e Troubleshooting**

### **Debug Visual**
1. Ative `Show Volume Info` no PostProcessingManager
2. Ative `Show Area Bounds` nas áreas
3. Visualize gizmos coloridos no Scene View

### **Problemas Comuns**

**❌ Efeitos não aparecem:**
- Verifique se URP está configurado
- Confirme que Post-Processing está habilitado na câmera
- Verifique se o Volume Profile tem efeitos ativos

**❌ Player não é detectado:**
- Confirme a Layer Mask do player
- Verifique se o Collider é Trigger
- Use Debug Logs para verificar detecção

**❌ Transições bruscas:**
- Aumente Transition Duration
- Verifique se volumes não estão sobrepostos
- Use Is Exclusive Area se necessário

## 📱 **Performance**

### **Otimizações**:
- Use volumes locais apenas onde necessário
- Desative efeitos não utilizados nos profiles
- Limite número de volumes simultâneos
- Use LOD para áreas distantes

### **Mobile**:
- Reduza intensidade dos efeitos
- Desative Bloom em dispositivos fracos
- Use profiles simplificados

## 🎯 **Exemplos de Uso**

### **Floresta Mística**
```csharp
// Área Normal → Magical → Dark
- Entrada: Bright (cores vibrantes)
- Centro: Magical (bloom, tons roxos)
- Profundezas: Dark (sombrio, vinheta)
```

### **Exploração de Caverna**
```csharp
// Superfície → Underground → Toxic
- Superfície: Normal
- Caverna: Underground (tons frios)
- Lago tóxico: Toxic (esverdeado)
```

### **Cidade Cyberpunk**
```csharp
// Diferentes distritos com visuais únicos
- Centro: Bright (neon, bloom)
- Subúrbios: Normal
- Esgoto: Toxic
- Laboratório: Magical
```

## 📚 **Referências Técnicas**

- [Unity URP Post-Processing](https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@14.0/manual/post-processing/post-processing-intro.html)
- [Volume System](https://docs.unity3d.com/Packages/com.unity.render-pipelines.core@14.0/manual/Volumes.html)
- [URP Camera Settings](https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@14.0/manual/cameras-advanced.html)

---

## 🏗️ **Próximos Passos**

1. **Configure** o PostProcessingManager na sua cena
2. **Crie** duas áreas de teste (Dark e Bright)
3. **Teste** as transições movendo o player
4. **Customize** os efeitos conforme necessário
5. **Otimize** para sua plataforma alvo

**Agora você tem um sistema completo de ambientação visual baseado em áreas!** 🎨✨ 