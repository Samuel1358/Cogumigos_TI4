# 🚀 Guia Rápido - Duas Áreas com Post-Processing

## ✅ **Pré-requisitos (IMPORTANTE!)**

1. **Seu projeto deve usar URP (Universal Render Pipeline)**
2. **Sua câmera deve ter Post-Processing habilitado**
3. **Seu player deve ter a tag "Player"**

---

## 🎯 **Passo a Passo Simples**

### **1. Configurar a Câmera**
- Selecione sua câmera principal
- No inspector, procure por **"Rendering"**
- Marque **"Post Processing"** ✅

### **2. Configurar o Player**
- Selecione seu GameObject do player
- No topo do inspector, mude a **Tag** para **"Player"**

### **3. Criar Área Iluminada (Alegre)**

1. **Criar GameObject:**
   - Clique direito na Hierarchy → Create Empty
   - Renomeie para: `Área Alegre`

2. **Adicionar Componentes:**
   - Add Component → `Box Collider`
   - Marque **"Is Trigger"** ✅
   - Add Component → `Volume` (Rendering)
   - Add Component → `Simple Post Processing Area`

3. **Configurar o Script:**
   - **Area Name**: "Área Alegre"
   - **Area Type**: `Bright`
   - **Player Tag**: "Player"

4. **Posicionar:**
   - Mova o GameObject para onde quer a área alegre
   - Ajuste o **Box Collider Size** para cobrir a área desejada

### **4. Criar Área Escura**

1. **Criar GameObject:**
   - Clique direito na Hierarchy → Create Empty
   - Renomeie para: `Área Escura`

2. **Adicionar Componentes:**
   - Add Component → `Box Collider`
   - Marque **"Is Trigger"** ✅
   - Add Component → `Volume` (Rendering)
   - Add Component → `Simple Post Processing Area`

3. **Configurar o Script:**
   - **Area Name**: "Área Escura"
   - **Area Type**: `Dark`
   - **Player Tag**: "Player"

4. **Posicionar:**
   - Mova o GameObject para onde quer a área escura
   - Ajuste o **Box Collider Size** para cobrir a área desejada

---

## 🎨 **O que cada área faz:**

### **☀️ Área Bright (Alegre):**
- ✨ **Bloom** suave para brilho
- 🌈 **Cores mais vibrantes** e saturadas
- ☀️ **Mais iluminada** (+30% exposure)
- 🟡 **Vinheta dourada** suave

### **🌑 Área Dark (Escura):**
- 🔻 **Mais escura** (-60% exposure)
- ⚫ **Dessaturada** (-25% saturação)
- ⬛ **Vinheta preta** intensa
- 🔵 **Tom azulado** nas sombras

---

## 🧪 **Testar o Sistema**

1. **Play** na cena
2. **Mova o player** para dentro das áreas
3. **Observe** as mudanças visuais com transições suaves
4. **Verifique o Console** para logs de debug

### **Teste Manual (sem mover o player):**
- Selecione uma área no inspector
- Clique direito no script → **"Test Enter Area"**
- Para sair: **"Test Exit Area"**

---

## 🐛 **Problemas Comuns**

### **❌ "Não funciona nada"**
- ✅ Verifique se está usando **URP**
- ✅ Verifique se **Post-Processing** está habilitado na câmera
- ✅ Verifique se o player tem tag **"Player"**

### **❌ "Player não é detectado"**
- ✅ Confirme que o **Box Collider** está marcado como **"Is Trigger"**
- ✅ Confirme que o player tem a tag correta
- ✅ Verifique se o collider está grande o suficiente

### **❌ "Efeitos muito fracos"**
- ✅ Aumente o **Transition Duration** para ver melhor
- ✅ Teste com **"Test Enter Area"** no inspector

---

## ⚡ **Configuração Rápida (2 minutos)**

```
1. Câmera → Post-Processing ✅
2. Player → Tag "Player" ✅
3. Create Empty → "Área Alegre"
   - Box Collider (Trigger ✅)
   - Volume
   - SimplePostProcessingArea (Type: Bright)
4. Create Empty → "Área Escura"  
   - Box Collider (Trigger ✅)
   - Volume
   - SimplePostProcessingArea (Type: Dark)
5. Posicionar as áreas na cena
6. Play e testar!
```

**Pronto! Agora você tem duas áreas com efeitos visuais diferentes! 🎉** 