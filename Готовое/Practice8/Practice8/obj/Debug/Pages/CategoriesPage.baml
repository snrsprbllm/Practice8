   M S B A M L   `   `   `  ����  	Practice8$    Practice8.Pages.CategoriesPage   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5        N� Title$  Категории��5       N� 
Background
 �����.��5      � $��10q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5	      $��* �6   �� 5
      $
��Auto �6   .+�5   
   �� -   5         ��Name$ CategoriesListView��5      $��1��6/   $��0,0,0,10q�6<    �� View 5      �� 5      .P�5      ��  �� Width$ 50��6I    �� DisplayMemberBinding 6%   �� *id+.��5      �� -   5        $��ID��5   /   
 � Tag$ id��6<   6E   5      �� 5      $	 200��6K    6%   �� *name+.��5      �� -   5        $��Название��5   /   $
 name��6B   6M   5      5      5      �� 5   
    �� Orientation$ 
Horizontal=�6   $��2��6.   $��Right=�6;   .+�5      �� -   5        $��Добавить��5      $	��100��6(   $��5q�64   6?   �� 5      -   5        $ ��Редактировать��5      $	��100��6-   $��5q�69   6D   �� 5      -   5        $��Удалить��5      $	��100��6'   $��5q�63   6>   5       5!      5"      