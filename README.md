ARC
===

# unity3D 的小工具包

可只抓 ARC.unitypackage , 解開後東西全放在 Assets/ARCRoot/ 裡


目錄結構
------------

* ARCRoot (根目錄)
  * ARC (主要程式碼)
    * Code (主要程式碼)
    * Editor (編輯器功能)
* Demo (範例)


元件尋找工具
------------

一個視窗, 輸入條件, 找尋物件.

1. Window -> ARC -> Find
2. Add Condition : 加入一個要尋找條件, 如腳本名稱.
 * 輸入名稱
 * 選擇種類 (Componet, Tag, Layer, SortingLayer)
3. Clear : 清空尋找條件
4. FindHierachy : 在 Hierarchy 裡找出符合條件的 GameObject
5. FindAsset : 在 Assets 裡找出符合條件的 Prefab
  

其它未完成
-----------

* Timer
* Debug log

