   M S B A M L   `   `   `  ����?  ;Prop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:PropProp  5   	       Prop.MainTorneo   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   $localclr-namespace:Prop     5   	   -   5          �Name$  mainTorneos��5   	    � Title$ Selecciona un torneo��5   	   $	��200��6&   $	��400��63    � 
ResizeMode$ NoResize=�6?    � WindowStartupLocation$ CenterScreen=�6U    � Icon$ imagenes/balon.png��6z   ���	#FFFDFDFD��5	      �� 5
   
    �� EndPoint$ 0.5,1&�6    �� 
StartPoint$ 0.5,0&�6/   .��5      �  � Color$ 	#FF84DCC6��6   � 5      $ 	#FF4B4E6D��6    � Offset$ 1Q�6-   5      .��5      � 	 � ColumnDefinitions	 5   
   �� 5      �� 5      $��0* �6   .+�5   
   �� -   5        
 ��Name$
 	cmbtorneo��5      $
��Left=�6&   $��	96,45,0,0q�6A   $	��Top=�6T   $	��198��6l    J� RenderTransformOrigin$ 0.196,-4.568&�6x   $ �� IsSynchronizedWithCurrentItem$ FalseY�6�   ���	#FFF9F9F9$��22��6  ��5      �� 5      $	 0,1&�6&   $	 0,0&�65   .��5      � $ 	#FFF0F0F0��6#   $ 0Q�65   � 5      $ 	#FF34675C��6#   $ 1Q�65   5      5      �� 5   
   -   5         ��Name$ btnAceptarTorneo��5      $��Cargar Torneo��6+   $
��Left=�6C   $��
22,116,0,0q�6^   $	��Top=�6r   $	��107��6�   ���	#FFD8E0CB$��20��6�    � 	Resources 5       �� 
TargetType �� Property �� Value��%^   '��       �� 5      " ��6   .�5      ��  �@��CornerRadius$ 8��65   5       5!      5"      �� 5#   
   -   5        $ btnEliminarTorneo��5#      $��Eliminar Torneo��6,   $
��Left=�6F   $��143,116,0,0q�6a   $	��Top=�6v   $	��107��6�   ���	#FFD8E0CB$��20��6�    5$      ��%^   '��       �� 5%      " ��6   .�5&      ��  �@��CornerRadius$ 8��65   5'      5(      5)      �� 5*   
   -   5        $ btnModificaTorneo��5*      $��Modifica Torneo��6,   $
��Left=�6F   $��263,115,0,0q�6a   $	��Top=�6v   $	��107��6�   ���	#FFD8E0CB$��20��6�    5+      ��%^   '��       �� 5,      " ��6   .�5-      ��  �@��CornerRadius$ 8��65   5.      5/      50      51      52      