   M S B A M L   `   `   `  ����  	Practice8#    Practice8.Pages.OrderEditPage   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5        N� Title$F  @Добавление/Редактирование платежа��5       N� 
Background
 �����.��5      � $��10q�6    � RowDefinitions 5   
   �� 5      $��* �6   �� 5	      $
��Auto �6   .+�5   
   �� .+�5      �� $��Продукт��6   $
��Bold"�6'   �� 5      -   5         ��Name$ ProductComboBox��5       �� DisplayMemberPath$
 name��60    �� SelectedValuePath$ id��6I   $	��0,5q�6`   �� 5      $��Пользователь��6   $
��Bold"�6,   �� 5      -   5        $ UserComboBox��5      $ 	last_name��6-   $ id��6K   $	��0,5q�6b   �� 5      $��Цена��6   $
��Bold"�6$   �� 5      -   5         ��Name$ PriceTextBox��5      $	��0,5q�6,   �� 5      $��Количество��6   $
��Bold"�6*   �� 5      -   5        $ CountTextBox��5      $	��0,5q�6,   �� 5      $��Дата��6   $
��Bold"�6$   (  "System.Windows.Controls.DatePicker  5      -   5          Name$ 
DatePicker��5      $	��0,5q�6-   �� 5      -   5         ��Name$ InfoTextBlock��5      
���  ��$
��0,10q�6@   5      �� 5   
   $��1��6   	 �� Orientation$	 
Horizontal=�6"   $��Right=�6;   .+�5      �� -   5        $��Сохранить��5      $��5q�6)   64   �� 5      -   5        $��Отмена��5      $��5q�6&   61   5      5       5!      