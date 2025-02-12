using System.Collections.Generic;

public static class LocalizationKeyStrings
{
    public static Dictionary<LocalizationKeys, string> KeyMappings = new Dictionary<LocalizationKeys, string>
    {        { LocalizationKeys.ACTION_CONFIRM_TITLE, "ACTION.CONFIRM_TITLE" },
        { LocalizationKeys.ACTION_CONFIRM_TRANSFER_ATK_DEF_MESSAGE, "ACTION.CONFIRM_TRANSFER_ATK_DEF_MESSAGE" },
        { LocalizationKeys.ACTION_CONTINUE_APPLY_FAMILY_MESSAGE, "ACTION.CONTINUE_APPLY_FAMILY_MESSAGE" },
        { LocalizationKeys.ACTION_SKIP_DRAW_FOR_FISTILAND_CARD_MESSAGE, "ACTION.SKIP_DRAW_FOR_FISTILAND_CARD_MESSAGE" },
        { LocalizationKeys.ACTION_TITLE, "ACTION.TITLE" },
        { LocalizationKeys.BUTTON_BACK, "BUTTON.BACK" },
        { LocalizationKeys.BUTTON_CONTRE, "BUTTON.CONTRE" },
        { LocalizationKeys.BUTTON_DETAILS, "BUTTON.DETAILS" },
        { LocalizationKeys.BUTTON_EQUIP_INVOCATION, "BUTTON.EQUIP_INVOCATION" },
        { LocalizationKeys.BUTTON_HAND, "BUTTON.HAND" },
        { LocalizationKeys.BUTTON_NO, "BUTTON.NO" },
        { LocalizationKeys.BUTTON_OK, "BUTTON.OK" },
        { LocalizationKeys.BUTTON_PUT_CARD, "BUTTON.PUT_CARD" },
        { LocalizationKeys.BUTTON_YES, "BUTTON.YES" },
        { LocalizationKeys.CARD_CHOICE_BUTTON_PLAYER_ONE, "CARD_CHOICE.BUTTON.PLAYER_ONE" },
        { LocalizationKeys.CARD_CHOICE_BUTTON_PLAYER_TWO, "CARD_CHOICE.BUTTON.PLAYER_TWO" },
        { LocalizationKeys.CARD_CHOICE_TITLE_PLAYER_ONE, "CARD_CHOICE.TITLE.PLAYER_ONE" },
        { LocalizationKeys.CARD_CHOICE_TITLE_PLAYER_TWO, "CARD_CHOICE.TITLE.PLAYER_TWO" },
        { LocalizationKeys.CARDS_SELECTOR_TITLE_CHANGE_ORDER_CARTES, "CARDS_SELECTOR_TITLE.CHANGE_ORDER_CARTES" },
        { LocalizationKeys.CARDS_SELECTOR_TITLE_CHOICE_CARD_FROM_DECK_YELLOW, "CARDS_SELECTOR_TITLE.CHOICE_CARD_FROM_DECK_YELLOW" },
        { LocalizationKeys.CARDS_SELECTOR_TITLE_CHOICE_CONTROLLED_INVOCATION, "CARDS_SELECTOR_TITLE.CHOICE_CONTROLLED_INVOCATION" },
        { LocalizationKeys.CARDS_SELECTOR_TITLE_CHOICE_DESTROY_CARD, "CARDS_SELECTOR_TITLE.CHOICE_DESTROY_CARD" },
        { LocalizationKeys.CARDS_SELECTOR_TITLE_CHOICE_DESTROY_FIELD_CARD, "CARDS_SELECTOR_TITLE.CHOICE_DESTROY_FIELD_CARD" },
        { LocalizationKeys.CARDS_SELECTOR_TITLE_CHOICE_FAMILY_CARD, "CARDS_SELECTOR_TITLE.CHOICE_FAMILY_CARD" },
        { LocalizationKeys.CARDS_SELECTOR_TITLE_CHOICE_FIELD, "CARDS_SELECTOR_TITLE.CHOICE_FIELD" },
        { LocalizationKeys.CARDS_SELECTOR_TITLE_CHOICE_INVOCATION_FOR_EQUIPMENT, "CARDS_SELECTOR_TITLE.CHOICE_INVOCATION_FOR_EQUIPMENT" },
        { LocalizationKeys.CARDS_SELECTOR_TITLE_CHOICE_INVOKE, "CARDS_SELECTOR_TITLE.CHOICE_INVOKE" },
        { LocalizationKeys.CARDS_SELECTOR_TITLE_CHOICE_RECEIVER_CARD, "CARDS_SELECTOR_TITLE.CHOICE_RECEIVER_CARD" },
        { LocalizationKeys.CARDS_SELECTOR_TITLE_CHOICE_REMOVE_CARDS_FROM_HAND, "CARDS_SELECTOR_TITLE.CHOICE_REMOVE_CARDS_FROM_HAND" },
        { LocalizationKeys.CARDS_SELECTOR_TITLE_CHOICE_SACRIFICE, "CARDS_SELECTOR_TITLE.CHOICE_SACRIFICE" },
        { LocalizationKeys.CARDS_SELECTOR_TITLE_CHOICE_SACRIFICES, "CARDS_SELECTOR_TITLE.CHOICE_SACRIFICES" },
        { LocalizationKeys.CARDS_SELECTOR_TITLE_CHOICE_SET_FIELD, "CARDS_SELECTOR_TITLE.CHOICE_SET_FIELD" },
        { LocalizationKeys.CARDS_SELECTOR_TITLE_CHOICE_SKIP_ATTACK, "CARDS_SELECTOR_TITLE.CHOICE_SKIP_ATTACK" },
        { LocalizationKeys.CARDS_SELECTOR_TITLE_CHOOSE_OPPONENT, "CARDS_SELECTOR_TITLE.CHOOSE_OPPONENT" },
        { LocalizationKeys.CARDS_SELECTOR_TITLE_DEFAULT_CHOICE_CARD, "CARDS_SELECTOR_TITLE.DEFAULT_CHOICE_CARD" },
        { LocalizationKeys.CARDS_SELECTOR_TITLE_OPPONENT_CARDS, "CARDS_SELECTOR_TITLE.OPPONENT_CARDS" },
        { LocalizationKeys.CARDS_SELECTOR_TITLE_REMOVE_CARD_FROM_HAND, "CARDS_SELECTOR_TITLE.REMOVE_CARD_FROM_HAND" },
        { LocalizationKeys.CARDS_SELECTOR_TITLE_REMOVE_CARD_FROM_OPPONENT_HAND, "CARDS_SELECTOR_TITLE.REMOVE_CARD_FROM_OPPONENT_HAND" },
        { LocalizationKeys.FAMILY_COMICS, "FAMILY.COMICS" },
        { LocalizationKeys.FAMILY_DEVELOPER, "FAMILY.DEVELOPER" },
        { LocalizationKeys.FAMILY_FISTILAND, "FAMILY.FISTILAND" },
        { LocalizationKeys.FAMILY_HARD_CORNER, "FAMILY.HARD_CORNER" },
        { LocalizationKeys.FAMILY_HUMAN, "FAMILY.HUMAN" },
        { LocalizationKeys.FAMILY_INCARNATION, "FAMILY.INCARNATION" },
        { LocalizationKeys.FAMILY_JAPAN, "FAMILY.JAPAN" },
        { LocalizationKeys.FAMILY_MONSTER, "FAMILY.MONSTER" },
        { LocalizationKeys.FAMILY_POLICE, "FAMILY.POLICE" },
        { LocalizationKeys.FAMILY_RPG, "FAMILY.RPG" },
        { LocalizationKeys.FAMILY_SPATIAL, "FAMILY.SPATIAL" },
        { LocalizationKeys.FAMILY_WIZARD, "FAMILY.WIZARD" },
        { LocalizationKeys.INFORMATION_NO_CARD_GET_FROM_DECK_MESSAGE, "INFORMATION.NO_CARD_GET_FROM_DECK_MESSAGE" },
        { LocalizationKeys.INFORMATION_NO_DESTROY_CARD_MESSAGE, "INFORMATION.NO_DESTROY_CARD_MESSAGE" },
        { LocalizationKeys.INFORMATION_NO_FIELD_CARD_SET_MESSAGE, "INFORMATION.NO_FIELD_CARD_SET_MESSAGE" },
        { LocalizationKeys.INFORMATION_NO_INVOKED_CARD_MESSAGE, "INFORMATION.NO_INVOKED_CARD_MESSAGE" },
        { LocalizationKeys.INFORMATION_NO_SKIP_ATTACK_MESSAGE, "INFORMATION.NO_SKIP_ATTACK_MESSAGE" },
        { LocalizationKeys.INFORMATION_NO_WIN_STARS_MESSAGE, "INFORMATION.NO_WIN_STARS_MESSAGE" },
        { LocalizationKeys.INFORMATION_OPPONENT_CANT_ATTACK_MESSAGE, "INFORMATION.OPPONENT_CANT_ATTACK_MESSAGE" },
        { LocalizationKeys.INFORMATION_TITLE, "INFORMATION.TITLE" },
        { LocalizationKeys.MODIFY_DECK_MESSAGE, "MODIFY_DECK.MESSAGE" },
        { LocalizationKeys.MODIFY_DECK_TITLE, "MODIFY_DECK.TITLE" },
        { LocalizationKeys.PAUSE_MESSAGE, "PAUSE.MESSAGE" },
        { LocalizationKeys.PAUSE_TITLE, "PAUSE.TITLE" },
        { LocalizationKeys.PHASE_ATTACK, "PHASE.ATTACK" },
        { LocalizationKeys.PHASE_CHOOSE, "PHASE.CHOOSE" },
        { LocalizationKeys.PHASE_DRAW, "PHASE.DRAW" },
        { LocalizationKeys.PLAYER_ONE, "PLAYER.ONE" },
        { LocalizationKeys.PLAYER_TWO, "PLAYER.TWO" },
        { LocalizationKeys.QUESTION_CHOICE_SEE_DECK_MESSAGE, "QUESTION.CHOICE_SEE_DECK_MESSAGE" },
        { LocalizationKeys.QUESTION_CHOICE_TITLE, "QUESTION.CHOICE_TITLE" },
        { LocalizationKeys.QUESTION_DESTROY_OPPONENT_INVOCATION_MESSAGE, "QUESTION.DESTROY_OPPONENT_INVOCATION_MESSAGE" },
        { LocalizationKeys.QUESTION_DIVIDE_TO_DESTROY_FIELD_MESSAGE, "QUESTION.DIVIDE_TO_DESTROY_FIELD_MESSAGE" },
        { LocalizationKeys.QUESTION_DRAW_CARDS_MESSAGE, "QUESTION.DRAW_CARDS_MESSAGE" },
        { LocalizationKeys.QUESTION_GET_CARD_FROM_FAMILY_MESSAGE, "QUESTION.GET_CARD_FROM_FAMILY_MESSAGE" },
        { LocalizationKeys.QUESTION_GET_SPECIFIC_CARD_IN_DECK_AND_YELLOW_MESSAGE, "QUESTION.GET_SPECIFIC_CARD_IN_DECK_AND_YELLOW_MESSAGE" },
        { LocalizationKeys.QUESTION_GET_SPECIFIC_CARD_IN_DECK_MESSAGE, "QUESTION.GET_SPECIFIC_CARD_IN_DECK_MESSAGE" },
        { LocalizationKeys.QUESTION_GET_TYPE_CARD_MESSAGE, "QUESTION.GET_TYPE_CARD_MESSAGE" },
        { LocalizationKeys.QUESTION_INVOKE_NON_COLLECTOR_BY_SACRFICE_MESSAGE, "QUESTION.INVOKE_NON_COLLECTOR_BY_SACRFICE_MESSAGE" },
        { LocalizationKeys.QUESTION_INVOKE_SPECIFIC_CARD_MESSAGE, "QUESTION.INVOKE_SPECIFIC_CARD_MESSAGE" },
        { LocalizationKeys.QUESTION_OR_OPTION, "QUESTION.OR_OPTION" },
        { LocalizationKeys.QUESTION_REMOVE_CARD_OPPONENT_HAND_MESSAGE, "QUESTION.REMOVE_CARD_OPPONENT_HAND_MESSAGE" },
        { LocalizationKeys.QUESTION_SACRIFICE_TO_BOOST_MESSAGE, "QUESTION.SACRIFICE_TO_BOOST_MESSAGE" },
        { LocalizationKeys.QUESTION_SET_FIELD_CARD_MESSAGE, "QUESTION.SET_FIELD_CARD_MESSAGE" },
        { LocalizationKeys.QUESTION_SKIP_OPPONENT_ATTACK_MESSAGE, "QUESTION.SKIP_OPPONENT_ATTACK_MESSAGE" },
        { LocalizationKeys.QUESTION_TITLE, "QUESTION.TITLE" },
        { LocalizationKeys.TOAST_ASK, "TOAST_ASK" },
        { LocalizationKeys.TYPE_CONTRE, "TYPE.CONTRE" },
        { LocalizationKeys.TYPE_EFFECT, "TYPE.EFFECT" },
        { LocalizationKeys.TYPE_EQUIPMENT, "TYPE.EQUIPMENT" },
        { LocalizationKeys.TYPE_FIELD, "TYPE.FIELD" },
        { LocalizationKeys.TYPE_INVOCATION, "TYPE.INVOCATION" },
        { LocalizationKeys.TYPE_CARD, "TYPE_CARD" },
        { LocalizationKeys.WARNING_CANNOT_ATTACK_MESSAGE, "WARNING.CANNOT_ATTACK_MESSAGE" },
        { LocalizationKeys.WARNING_LIMIT_COLLECTOR_CARD, "WARNING.LIMIT_COLLECTOR_CARD" },
        { LocalizationKeys.WARNING_LIMIT_EFFECT_CARDS, "WARNING.LIMIT_EFFECT_CARDS" },
        { LocalizationKeys.WARNING_LIMIT_NUMBER_CARDS, "WARNING.LIMIT_NUMBER_CARDS" },
        { LocalizationKeys.WARNING_MUST_CHOOSE_CARD, "WARNING.MUST_CHOOSE_CARD" },
        { LocalizationKeys.WARNING_MUST_CHOOSE_CARDS, "WARNING.MUST_CHOOSE_CARDS" },
        { LocalizationKeys.WARNING_MUST_CHOOSE_FIELD_CARD, "WARNING.MUST_CHOOSE_FIELD_CARD" },
        { LocalizationKeys.WARNING_MUST_CHOOSE_SACRIFICE, "WARNING.MUST_CHOOSE_SACRIFICE" },
        { LocalizationKeys.WARNING_MUST_CHOOSE_SACRIFICES, "WARNING.MUST_CHOOSE_SACRIFICES" },
        { LocalizationKeys.WARNING_MUST_ORDER_CARDS, "WARNING.MUST_ORDER_CARDS" },
        { LocalizationKeys.WARNING_MUST_REMOVE_CARDS, "WARNING.MUST_REMOVE_CARDS" },
        { LocalizationKeys.WARNING_TITLE, "WARNING.TITLE" },
        { LocalizationKeys.YOUR_ATTACK, "YOUR_ATTACK" },
        { LocalizationKeys.YOUR_DEFENSE, "YOUR_DEFENSE" },
    };
}
