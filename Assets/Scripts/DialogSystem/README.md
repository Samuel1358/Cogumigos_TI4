# Sistema de Diálogo

Este é um sistema completo de diálogos para Unity que permite criar conversas entre personagens usando ScriptableObjects.

## Características

- Criação de diálogos usando ScriptableObjects
- Exibição de retratos de personagens
- Sistema de digitação de texto animado
- Interface de usuário customizável
- Interação com objetos no mundo do jogo
- Editor personalizado para facilitar a criação de diálogos

## Configuração da UI

1. Crie um Canvas UI em sua cena
2. Adicione um GameObject vazio e renomeie-o para "DialogPanel"
3. Adicione o componente `DialogUI` ao "DialogPanel"
4. Crie a hierarquia UI conforme abaixo:
   - DialogPanel (GameObject)
     - Background (Image)
     - PortraitFrame (Image)
       - Portrait (Image) - Arraste este para o campo "Speaker Portrait" no componente DialogUI
     - NamePanel (Image)
       - NameText (TextMeshProUGUI) - Arraste este para o campo "Speaker Name Text" no componente DialogUI
     - MessagePanel (Image)
       - MessageText (TextMeshProUGUI) - Arraste este para o campo "Message Text" no componente DialogUI

5. Arraste o GameObject "DialogPanel" para o campo "Dialog Panel" no componente DialogUI

## Criação de Diálogos

1. Clique com o botão direito no Project Window
2. Navegue até Create > Dialog System > Dialog Data
3. Um novo arquivo DialogData será criado
4. Selecione o arquivo e use o editor personalizado para adicionar linhas de diálogo
5. Para cada linha de diálogo, defina:
   - Nome do personagem
   - Retrato do personagem (opcional)
   - Mensagem do diálogo

## Usando Diálogos no Jogo

### Configuração do controlador de diálogos

1. Adicione o componente `DialogController` a um GameObject em sua cena (de preferência um objeto que persista por toda a cena)
2. Arraste a referência do `DialogUI` para o campo apropriado no `DialogController`

### Iniciando Diálogos por Interação

1. Adicione o componente `DialogInteractable` a qualquer objeto com o qual o jogador possa interagir
2. Defina o `DialogData` a ser usado quando o jogador interagir com este objeto
3. Defina a tecla de interação (padrão é E)
4. Certifique-se de que o objeto tem um Collider com "Is Trigger" marcado
5. Certifique-se de que o objeto do jogador esteja na layer "Player"

### Iniciando Diálogos por Script

```csharp
public class MyGameController : MonoBehaviour
{
    [SerializeField] private DialogData myDialog;
    [SerializeField] private DialogController dialogController;

    public void StartMyDialog()
    {
        dialogController.StartDialog(myDialog);
    }
}
```

## Exemplos

Crie vários tipos de diálogos diferentes para testar o sistema:

1. Diálogo simples entre dois personagens
2. Monólogo de um personagem
3. Diálogo com múltiplos personagens

## Dicas

- Você pode definir a velocidade de animação do texto no componente DialogUI
- Você pode personalizar a aparência da UI para combinar com o estilo visual do seu jogo
- Use eventos OnDialogStarted e OnDialogEnded para controlar comportamentos como pausar o jogo durante diálogos
