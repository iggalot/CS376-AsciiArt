   M S B A M L   `   `   `  ����C  ?AsciiArt, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:AsciiArtAsciiArt  5   	   O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5   	       AsciiArt.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:AsciiArt     5   	   6sys&clr-namespace:System;assembly=mscorlib     5   	   -   5          � Title$  Ascii Art Inverter��5	   	    � SizeToContent$ WidthAndHeight=�6$    � 	Resources 5         strHelloWorld��%/   &	        ��5   
   Hello, world!��6:   5      .��5      � $
��Auto��6   $
��Auto��6   .+�5   
   �� $	��Top=�6   .+�5      o� $
��Auto��6   $��20��6    ���	#FFA9D1F4$	��Top=�6C   .@�5      m� $��_File��6   .@�5      m� -   5         m�Name$ mnuLoad��5      $��_Open��60   $
��Left=�6[   6x   m� 5      -   5        $ 	mnuSaveAs��5      $��_Save As��62   $
��Left=�6]    m� Icon 5      �� 5      $��icons/SaveAs_16x_32.bmp��6$   6G   5      �� 5      6"   m� 5      -   5        $ AppExit��5      $��E_xit��60   $
��Left=�6?    5      �� 5      $$��icons/Close_NoHalo_12x_16x.png��6$   6N   5      5      m� 5       $��_Font��6   .@�5!      m� -   5        $ mnuFont��5!      $��_Change Font��6G   $
��Left=�6]   $	��200��6x   6�   �� 5"      6"   5#      m� 5$      $��_Tools��6   .@�5%      m� -   5        $ mnuNegative��5%      $��	_Negative��6M   $
��Left=�6`   $	��200��6{   6�   m� 5&      -   5        $ mnuGrayscale��5&      $��
_Grayscale��6Q   $
��Left=�6e   $	��200��6�   6�   m� 5'      -   5        $ mnuGrayPixelated��5'      $��Gray Pixelated��6Y   $
��Left=�6q   $	��200��6�   6�   m� 5(      -	   5        $ mnuColorPixelated��5(      $��Color Pixelated��6[   $
��Left=�6t   $	��200��6�   6�   m� 5)      -
   5        $ mnuAsciiArt��5)      $��	Ascii Art��6O   $
��Left=�6b   $	��200��6}   6�   �� 5*      6"   m� 5+      -   5        $ mnuClearImage��5+      $��Clear Image��6S   $
��Left=�6h   $	��200��6�   6�   5,      m� 5-      $��_Help��6   .@�5.      m� -   5        $ _Help��5.      $��_Help��6.    m� InputGestureText$ F2��6T   $
��Left=�6j   $	��140��6�   6�   m� 5/      -   5        $��About��5/      $
��Left=�6F   $	��140��6a   50      51      �� 53       �� Orientation$ Vertical=�6   
�������.+�54      �� $ 
Horizontal=�6   
�������.+�55      �� $��Size: ��6   
�������
�������$��12(�6R   �� 56      -   5        $��
fontSlider��56      $��3q�6/   $	��150��6:   $
��Left=�6F    � Maximum$	 100Q�6a    � Minimum$ 2Q�6o   	 �� TickPlacement$	 BottomRight=�6{   
 �� TickFrequency$
 2Q�6�   6�   �� 57      -   5        $��
Draw ASCII��57      $��3q�62   6Z   �� 58      -   5        $��numBlockHorizLabel��58      $��#Horiz: ��66   $��Center=�6J   
�������
�������$��12(�6�   �� 59      -   5        $��strNumBlockHoriz��59       $��Center=�68   $
��Auto��6S   
�������
�������$��12(�6�   �� 5:      -   5        $��numBlockVertLabel��5:      $��#Vert: ��65   $��Center=�6H   
�������
�������$��12(�6�   �� 5;      -   5        $��strNumBlockVert��5;       $��Center=�67   $
��Auto��6R   
�������
�������$��12(�6�   5=      �� 5>      $ 
Horizontal=�6   
�������.+�5?      �� $��Ascii Char Qty: ��6   
�������
�������$��12(�6\   	� 5@      -   5         	�Name$ RBNormal��5@   "    	� 	GroupName$ RBASciiDensity��64   
�������$��Center=�6z   .��6�   Normal6�   	� 5A      -   5        $ RBLarge��5A   "   $ RBAsciiDensity��63   
�������$��Center=�6y   .��6�   Large6�   5B      5C      �� 5E      ��. .��5F      � .+�5G      �� $ 
Horizontal=�6!   
����  �.+�5H      �� -   5         ��Name$ sourcePhoto��5H       $��Center=�6c   $	��Top=�6�   $��5q�6�   $	��512��6�   �� 5I      -   5        $ convertedPhoto��5I       $��Center=�6d   $	��Top=�6�   $	��512��6�   $��5q�6�   5J      5K      5L      �� 5N      $ 
Horizontal=�6   
�������.+�5O      �� -   5        $��cursorPosition��5O      $��Center=�62   
�������
�������$��12(�6r   �� 5P      $ 
Horizontal=�6   $��Right=�66   
�������.+�5Q      �� -   5        $��lblCurrentFont��5Q      $��           CURRENT FONT: ��62   $	��135��6V   
�������
�������$��12(�6�   �� 5R      -   5        $��strCurrentFont��5R       $��Center=�66   $
��Auto��6Q   
�������
�������$��12(�6�   5S      5T      5V      5W      5X      