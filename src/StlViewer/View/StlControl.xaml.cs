using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using StlViewer.Model;

namespace StlViewer.View
{
    /// <summary>
    ///     Interaction logic for StlControl.xaml
    /// </summary>
    public partial class StlControl
    {
        #region Constructors

        public StlControl()
        {
            InitializeComponent();
        }

        #endregion

        #region Private Members

        private RotateTransform3D _rotateTransform;

        private Color _color;

        private void UpdateStlModel(StlModel stlModel)
        {
            Model3DGroup.Children.Clear();
            if (stlModel == null)
            {
                return;
            }
            var mesh = new MeshGeometry3D();
            var idPt = 0;
            foreach (var triangle in stlModel.Triangles)
            {
                mesh.Positions.Add(triangle.X);
                mesh.TriangleIndices.Add(idPt++);
                mesh.Positions.Add(triangle.Y);
                mesh.TriangleIndices.Add(idPt++);
                mesh.Positions.Add(triangle.Z);
                mesh.TriangleIndices.Add(idPt++);
            }
            _stlModel3D = new GeometryModel3D(mesh, new DiffuseMaterial(new SolidColorBrush(_color)));
            Model3DGroup.Children.Add(_stlModel3D);
            var range = new List<double>
            {
                stlModel.Max.X - stlModel.Min.X,
                stlModel.Max.Y - stlModel.Min.Y,
                stlModel.Max.Z - stlModel.Min.Z
            }.Max();
            var scale = Math.Abs(range) < float.Epsilon ? 1 : 1/range;
            var transform = new Transform3DGroup();
            transform.Children.Add(new TranslateTransform3D(-(stlModel.Max.X + stlModel.Min.X)/2,
                -(stlModel.Max.Y + stlModel.Min.Y)/2, -(stlModel.Max.Z + stlModel.Min.Z)/2));
            _scaleTransform = new ScaleTransform3D(scale, scale, scale);
            transform.Children.Add(_scaleTransform);
            _rotateTransform = new RotateTransform3D(new QuaternionRotation3D());
            transform.Children.Add(_rotateTransform);
            Model3DGroup.Transform = transform;
        }

        private void UpdateColor(Color color)
        {
            _color = color;
            if (_stlModel3D != null)
            {
                _stlModel3D.Material = new DiffuseMaterial(new SolidColorBrush(_color));
            }
        }

        #region Scale

        private void OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var newSacle = Math.Pow(1.1, e.Delta/120.0);
            _scaleTransform.ScaleX *= newSacle;
            _scaleTransform.ScaleY *= newSacle;
            _scaleTransform.ScaleZ *= newSacle;
        }

        #endregion

        #region Rotate

        private bool _isMouseDown;

        private Point _movePoint;

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (!_isMouseDown)
            {
                return;
            }
            var pt = e.GetPosition(Viewport3D);
            var minSize = Math.Min(Viewport3D.ActualWidth, Viewport3D.ActualHeight);
            var angleY = 360*(pt.X - _movePoint.X)/minSize;
            var angleX = 360*(pt.Y - _movePoint.Y)/minSize;
            var originalRotate = _rotateTransform.Rotation as QuaternionRotation3D;
            if (originalRotate != null)
            {
                _rotateTransform.Rotation =
                    new QuaternionRotation3D(new Quaternion(new Vector3D(1, 0, 0), angleX)*
                                             new Quaternion(new Vector3D(0, 1, 0), angleY)*originalRotate.Quaternion);
            }
            else
            {
                _rotateTransform.Rotation =
                    new QuaternionRotation3D(new Quaternion(new Vector3D(1, 0, 0), angleX)*
                                             new Quaternion(new Vector3D(0, 1, 0), angleY));
            }

            _movePoint = pt;
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            _isMouseDown = true;
            _movePoint = e.GetPosition(Viewport3D);
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            _isMouseDown = false;
        }

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            _isMouseDown = false;
        }

        #endregion

        #endregion

        #region Dependency Properties

        #region StlModel Property

        /// <summary>
        ///     Stl Model, this is a dependency property.
        /// </summary>
        public StlModel StlModel
        {
            get { return (StlModel) GetValue(StlModelProperty); }
            set { SetValue(StlModelProperty, value); }
        }

        public static void SetStlModel(StlControl control, StlModel model)
        {
            control.StlModel = model;
        }

        public static StlModel GetStlModel(StlControl control)
        {
            return control.StlModel;
        }

        /// <summary>
        ///     StlModel, don not update on members changed.
        /// </summary>
        public static DependencyProperty StlModelProperty = DependencyProperty.Register(
            nameof(StlModel), typeof(StlModel), typeof(StlControl),
            new FrameworkPropertyMetadata(StlModelChangedCallback));

        private ScaleTransform3D _scaleTransform;

        private static void StlModelChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var stlControl = d as StlControl;
            var stlModel = e.NewValue as StlModel;
            stlControl?.UpdateStlModel(stlModel);
        }

        #endregion

        #region Color Property

        /// <summary>
        ///     Color, this is a dependency property.
        /// </summary>
        public Color Color
        {
            get { return (Color) GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        public static void SetColor(StlControl control, Color color)
        {
            control.Color = color;
        }

        public static Color GetColor(StlControl control)
        {
            return control.Color;
        }

        /// <summary>
        ///     Color, don not update on members changed.
        /// </summary>
        public static DependencyProperty ColorProperty = DependencyProperty.Register(
            nameof(Color), typeof(Color), typeof(StlControl),
            new FrameworkPropertyMetadata(ColorChangedCallback));

        private GeometryModel3D _stlModel3D;

        private static void ColorChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var stlControl = d as StlControl;
            if (!(e.NewValue is Color))
            {
                return;
            }
            stlControl?.UpdateColor((Color) e.NewValue);
        }

        #endregion

        #endregion
    }
}