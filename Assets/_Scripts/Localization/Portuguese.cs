using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Portuguese : ILanguage
{
    public string Prefix { get { return "br"; } }

    public Dictionary<string, string> strings = new Dictionary<string, string>
    {
        { "PLAY","Jogue"},
        { "SURVIVAL_GAME" , "Sobrevivência" },
        { "TIMED_GAME" , "Temporizado" },
        { "SCORE" , "Pontuação" },
        { "OPTIONS" , "Opções" },
        { "TUTORIAL" , "Tutorial" },
        { "INVITE" , "Chamar mais amigos" },
        { "REMOVE_ADS" , "Remover Propagandas" },
        { "SOUND" , "Som" },
        { "NOTIFICATION" , "Notificações" },
        { "LANGUAGE" , "Idioma" },
        { "RECORD_ON" , "Replay\nLigado" },       
        { "RECORD_OFF" , "Replay\nDesligado" },
        { "RESTORE_PURCHASES" , "Restaurar\nCompras" },
        { "LOGIN" , "Entrar" },
        { "LOGOUT" , "Sair" },
        { "FB_ASK_LOGIN","Clique no ícone abaixo para conectar ao facebook e ver os pontos dos seus amigos!"},
        { "FRIENDS" , "Amigos" },
        { "WORLD" , "Mundo" },
        { "LOADING" , "Carrengado" },
        {"CUSTOMIZE", "APARÊNCIA"},
        {"BG_COLOR", "COR DO FUNDO"},
        {"BUBBLE_FORM", "FORMA DA BOLHA"},
        {"CLOSE", "FECHAR"},
        {"MORE","MAIS"},
        {"EXTERNAL_LINK", "Cocê será para um link externo. Continue?" },


        {"TUTORIAL_1", "Ligue as bolhas para conseguir move-las"},
        {"TUTORIAL_2", "Junte duas bolhas para aumentar seus pontos"},
        {"TUTORIAL_3", "Fuja das caveiras, ou você pode perder um coração"},
        {"TUTORIAL_4", "Bolhas especiais fazem poderes serem ativados"},
        {"PRO_TIP_1", "DICA: Bolhas conectadas tem sua velocidade reduzida!"},
        {"PRO_TIP_2", "DICA: Bolhas com 10 pontos ou mais farão tudo explodir!"},
        {"PRO_TIP_3", "DICA: Duas bolhas de caveira podem colidir seguramente!"},
        {"PRO_TIP_4", "DICA: Bolha jogada fora da tela (laterais) é descartada!"},

        { "TIME" , "TEMPO" },
        { "LEVEL" , "Nível" },
        { "PAUSED" , "PAUSADO" },
        { "MENU" , "Menu" },
        { "RESUME" , "Despausar" },
        { "GAMEOVER" , "FIM DE JOGO" },
        { "RETRY" , "Recomeçar" },
        { "SHARE" , "Publicar" },
        { "REPLAY" , "Replay" },

        { "USE_LIFE" , "Usar uma vida?" },

        { "FB_FRIEND_GOT_YOUR_SCORE" , "Opa! {0} pegou o seu lugar na pontuação no modo {1}!" },
        { "REMOVED_FROM_TOP" , "Algúem bateu sua pontuação de melhor do mundo no modo {0} :( Pegue seu trono de volta!" },
        { "ASK_SEND_NOTIFICATION_FRIEND","Você passou os pontos de {0} ! Gostaria de enviar uma notificação?"},

        {"RATE_MY_APP","Boa jogada!\nVocê poderia, por favor, usar uns segundos para nos dar sua opnião?"},

        {"SCREENSHOT_TEXT", "Veja isso! Jogando Bubbit eu fiz {0} no modo {1}!"},

        {"ABOUT", "Sobre"},
        {"CONTACT_US","Contato"},
        {"SUPPORT", "Suporte"},
        {"EVERYPLAY_COM","Comunidade\nEveryplay"},
        {"FACEBOOK_PAGE","Bubbit no\nFacebook"},

        {"SHOP", "Loja"},
        {"FREE_COINS", "Moedas Grátis"},
        {"COINS_RECEIVED", "{0} moedas recebidas!"},

        {"REVIVE", "Reviver"},
        {"EXTRA_TIME", "Tempo Extra (30 secs)"},
        {"BUY_FOR", "Comprar por {0}"},
        {"NOT_ENOUGH", "Sem moedas suficientes.\nContinue jogando para ganhar mais!"},
        {"CONFIRM_PURCHASE", "Comprar por {0} moedas?"},

        {"GAME", "Game"},
        {"ACHIEVEMENTS", "Conquistas"},

        {"LEADERBOARDS", "Pontuações"},

    };

    public string GetString(string key)
    {
        if (strings.ContainsKey(key))
            return strings[key];

        return "NF: " + key;
    }
}
