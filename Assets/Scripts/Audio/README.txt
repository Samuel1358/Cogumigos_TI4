SISTEMA DE ÁUDIO - MANUAL DE USO
================================

Este sistema de áudio foi projetado para gerenciar todos os efeitos sonoros do jogo de forma organizada e fácil de usar.

1. CONFIGURAÇÃO INICIAL
-----------------------
1. Crie um objeto vazio na cena
2. Adicione o componente "AudioManager" a este objeto
3. No Inspector do AudioManager, configure:
   - A lista "Sound Effects" com todos os efeitos sonoros do jogo
   - Para cada efeito sonoro, defina:
     * Nome (use os nomes definidos em SoundEffectNames.cs)
     * Clip de áudio
     * Volume padrão (0 a 1)
     * Pitch padrão (0.1 a 3)

2. ADICIONANDO NOVOS EFEITOS SONOROS
-----------------------------------
1. Adicione o clip de áudio ao projeto (arraste para a pasta de áudios)
2. Abra o arquivo "SoundEffectNames.cs"
3. Adicione uma nova constante com o nome do efeito:
   public const string NOME_DO_SOM = "NomeDoSom";
4. No Inspector do AudioManager, adicione um novo item à lista "Sound Effects"
5. Configure o novo efeito sonoro com:
   - Nome: use a constante que você criou
   - Clip: selecione o arquivo de áudio
   - Volume e pitch desejados

3. USANDO O SISTEMA NO CÓDIGO
-----------------------------
Para tocar um efeito sonoro em qualquer script:

// Tocar um som globalmente
AudioManager.Instance.PlaySFX(SoundEffectNames.NOME_DO_SOM);

// Tocar um som em uma posição específica
AudioManager.Instance.PlaySFXAtPosition(SoundEffectNames.NOME_DO_SOM, transform.position);

// Tocar um som com volume e pitch personalizados
AudioManager.Instance.PlaySFX(SoundEffectNames.NOME_DO_SOM, volume: 0.8f, pitch: 1.2f);

4. CONTROLES ADICIONAIS
----------------------
// Parar todos os efeitos sonoros
AudioManager.Instance.StopSFX();

// Ajustar volume global dos efeitos sonoros
AudioManager.Instance.SetSFXVolume(0.5f);

// Ajustar pitch global dos efeitos sonoros
AudioManager.Instance.SetSFXPitch(1.2f);

5. BOAS PRÁTICAS
---------------
1. Sempre use as constantes de SoundEffectNames.cs para evitar erros de digitação
2. Mantenha os nomes dos efeitos sonoros organizados por categoria
3. Use PlaySFXAtPosition para efeitos 3D que precisam de posicionamento espacial
4. Ajuste o volume e pitch padrão no Inspector para cada efeito sonoro
5. Adicione novos efeitos sonoros seguindo o padrão de nomenclatura existente

6. EXEMPLOS DE USO
-----------------
// No script do jogador
void Jump()
{
    AudioManager.Instance.PlaySFX(SoundEffectNames.PLAYER_JUMP);
}

// No script de coletáveis
void OnCollect()
{
    AudioManager.Instance.PlaySFXAtPosition(
        SoundEffectNames.COIN_COLLECT,
        transform.position,
        volume: 0.7f
    );
}

// No script de UI
void OnButtonClick()
{
    AudioManager.Instance.PlaySFX(
        SoundEffectNames.BUTTON_CLICK,
        pitch: 1.1f
    );
} 