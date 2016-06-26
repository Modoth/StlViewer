# StlViewer

StlViewer is a wpf control for .stl file.

## Usage

1.Just binding `StlModel` to `StlModel` DependencyProperty of `StlControl`.
```xaml
<view:StlControl Color="Cornsilk"  StlModel="{Binding StlModel}" />
```
2.You can create `StlModel` by passing a data stream to the `StlModelParser`.
```csharp
var model = new StlParser().Parse(dataStream);
```
3.Rotate or scale the model with mouse.

## About

Simply as you can.

Any advice could also send to z_xueqin@icloud.com.
