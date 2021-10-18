# Class Tree:
<details>
  <summary></summary>
  This Is The Root Namespace. Anything Seen Here Can Already Be Accessed On Reference Without A "using" Directive.

- <details>
  <summary><b>PlagueButtonAPI</b></summary>
         This Is The NameSpace For The Button API. Creation Of Buttons, Toggles And Sliders Is Done In Here.
  
    - <details>
         <summary><b>ButtonAPI</b></summary>
         This Is The Class For The Button API. Creation Of Buttons, Toggles And Sliders Is Done In Here.

      - <details>
         <summary><b>Field: NewElementsMenuTransform</b></summary>
         The Main QuickMenu + /Container/Window/.
      ___
      - <details>
         <summary><b>Field: QuickMenuObj</b></summary>
         The Main QuickMenu.
      ___
        - <details>
         <summary><b>Field: UserInteractMenuTransform</b></summary>
         The Page When You Click On A User In-Game.
      ___
      - <details>
         <summary><b>Method: CreateButton(Transform Parent, string Text, string ToolTip, Action OnClick, Action< PlagueButton > OnCreation = null)</b></summary>
         This Is The Method To Create A Normal Button. When Calling This Method, Documentation Will Display As You Type The Method/Fill In The Arguments.
      ___
      - <details>
         <summary><b>Method: CreateToggle(Transform Parent, string Text, string ToolTip, Action<bool> OnToggle, bool DefaultState, bool RemoveButtonBackground, Color? CheckboxColour = null, Action< PlagueToggle > OnCreation = null)</b></summary>
         This Is The Method To Create A Toggle. When Calling This Method, Documentation Will Display As You Type The Method/Fill In The Arguments.
      ___
      - <details>
         <summary><b>Method: CreateSlider(Transform Parent, string Text, string ToolTip, Action<float> OnValueChanged, float DefaultValue, float MinPossibleValue, float MaxPossibleValue, Action< PlagueSlider > OnCreation = null)</b></summary>
         This Is The Method To Create A Toggle. When Calling This Method, Documentation Will Display As You Type The Method/Fill In The Arguments.
      ___
      - <details>
         <summary><b>Method: MakeEmptyPage(Wing wing, string name, string PageText, string PageTooltip, UIPage OptionalButtonParent = null, Action< PlaguePage > OnCreation = null)</b></summary>
         This Is The Method To Create A Empty Page For Adding Controls To. When Calling This Method, Documentation Will Display As You Type The Method/Fill In The Arguments.
      ___
      - <details>
         <summary><b>Component: ObjectHandler</b></summary>
         This Is A Component For Hooking To Generic Events Such As A Object Becoming Enabled, Disabled, Destroyed And For Events Such As Update. This Is Used Internally, But Is Public In Case You Need It.
  
    - <details>
         <summary><b>ButtonAPIExtensions</b></summary>
         This Is The Class For The Button API's Extensions, Such As Control Extensions.

      - <details>
         <summary><b>Extension Method: SetActive(this ButtonAPI.PlagueBase plagueBase, bool state)</b></summary>
         Sets The GameObject Active State To The State You Specify.
      ___
      - <details>
         <summary><b>Extension Method: SetInteractable(this ButtonAPI.PlagueButton plagueButton, bool interactable)</b></summary>
         Sets The Button Interactability State To The State You Specify. Also Toggles The CancelGif To Match.
      ___
      - <details>
         <summary><b>Extension Method: SetToggleState(this ButtonAPI.PlagueToggle plagueToggle, bool state)</b></summary>
         Sets The Toggle's Toggle State To The State You Specify. Does Not Invoke The OnToggle Event.
