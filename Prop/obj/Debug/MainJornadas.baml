   M S B A M L   `   `   `  ����?  ;Prop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:PropProp  5   	       Prop.MainJornadas   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   $localclr-namespace:Prop     5   	   -   5          � Title$  Jornadas��5   	   $	��450��6   $	��800��6'    � 
ResizeMode$ NoResize=�63    � Icon$ imagenes/balon.png��6`    � WindowStartupLocation$ CenterScreen=�6z   ��5	      �� 5
   
    �� EndPoint$ 0.5,1&�6    �� 
StartPoint$ 0.5,0&�6/   .��5      �  � Color$ 	#FF84DCC6��6    � Offset$ 0.003Q�6-   � 5      $ 	#FF4B4E6D��6   $ 0.997Q�6-   5      .��5      � .+�5   
   �� -   5         ��Name$ lbl_principal��5      $��Jornadas Completas��6'   $
��Left=�6D   $��
316,24,0,0q�6_   $	��Top=�6s   $��48(�6�   $$��Tw Cen MT Condensed Extra Bold)�6�   �� 5   
   -   5        $��cmb_jornadas��5      $
��Left=�6'   $��	16,46,0,0q�6B   $	��Top=�6U   $	��180��6m   6y   �� 5   
   -   5        $��lvJornadasPrincipal��5      $
��Left=�6.   $	��225��6I   $��
12,111,0,0q�6V   $	��Top=�6j   $	��680��6�   	 �� View	 5      �� 5      .P�5      �� $��Equipo Local��6%   
 �� Width$	
 200��6;    �� CellTemplate 5      �� 5      .R�5   "   ��  �� TextAlignment$ Center=�6C   $	��200��6Z   ��6,   �� *local+6Z   5      5      �� 5      $��Marcador Local��6%   $	
 110��6=    5      �� 5      .R�5    "   �� $ Center=�6F   $	��110��6]   ��6,   �� *
marlocal+6]   5!      5#      �� 5%      $��-��6%   $
 50��60    5&      �� 5'      .R�5(   "   �� $ Center=�6@   $��40��6W   ��6,   �� *vs+6W   5)      5+      �� 5-      $��Marcador Visitante��6%   $	
 110��6A    5.      �� 5/      .R�50   "   �� $ Center=�6J   $	��110��6a   ��6,   �� *marvisitante+6a   51      53      �� 55      $��Equipo Visitante��6%   $	
 200��6?    56      �� 57      .R�58   "   �� $ Center=�6G   $	��200��6^   ��6,   �� *	visitante+6^   59      5;      5=      5?      �� 5@   
   -   5        $
��Left=�5@      $��60��6+   $��705,345,0,0q�67   $	��Top=�6L   $��60��6d   ��5A      �� 5B       �� 
TargetType" ��6   .�5C      ��  �� Property � �� �� Value$ imagenes/undo.png��6/    �� Triggers 5D      P� 5E       P� Property �@J�IsMouseOver P� Value . . �5F      ��  � ��$  imagenes/undo_selected.png��67   5G      5I      5K      �� 5L   
   $
��Left=�6   $��75��6+   $��
705,24,0,0q�67   $	��Top=�6K   $��79��6c   $��imagenes/sombra2.png��6n   5N      5O      