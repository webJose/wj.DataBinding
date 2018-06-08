# wj.DataBinding

## English

Welcome.  This is a small .Net class library project that provides a couple of classes that assist in data binding scenarios by providing an implementation of the *INotifyPropertyChanged* as well as a *Container* class that can extend the properties of another class, also for the purposes of data binding.

The idea is super simple:  Extend a class by containment in the eyes of data-bound controls, such as the *DataGridView* grid control in Windows Forms, or the *DataGrid* control in Windows Presentation Foundation (WPF).

To see a more detailed explanation, head to the [Wiki](https://github.com/webJose/wj.DataBinding/wiki).

## Español

Bienvenid@.  Este proyecto es una pequeña biblioteca de clases que provee un par de clases que asisten en escenarios de vinculación a datos proveyendo una implementación de la interfaz *INotifyPropertyChanged* así como también una clase *Container* que puede extender las propiedades de otra clase, también para el propósito de vinculación a datos.

La idea es súper sencilla:  Extender una clase vía contención en los ojos de un control vinculado a datos, tal como el control grilla *DataGridView* en Windows Forms, o el control grilla *DataGrid* en Windows Presentation Foundation (WPF).

Para ver una explicación más detallada, dirígase al [Wiki](https://github.com/webJose/wj.DataBinding/wiki).

## English (upcoming version)

Welcome.  This is a small .Net class library project that provides an assorted set of classes that are meant to help in data binding scenarios.

* **NotifyPropertyChanged**:  This is an implementation of the *INotifyPropertyChanged* interface.  It provides a simple function that can be used to both save and notify of property changes.  The function returns a Boolean value indicating if change occurred, which can be used to further chain notifications of, say, calculated properties.
* **Container**:  It is a generic class that can extend another for the purposes of data binding.  Any properties added to classes that derive from this class are grouped together with the properties of a contained object.  Then the entire set of properties are presented together as one to data-bound controls such as the Windows Forms *DataGridView* control or the WPF *DataGrid* control.
* **ObservableCollectionEx**:  This is an enhanced *ObservableCollection* class.  It allows deactivation (called freezing) of the collection changed event, and later reactivation (unfreezing) with automatic *CollectionChanged* event firing.  It is useful in several scenarios, such as when a control is bound to a collection and the collection then receives many elements.  In the stock *ObservableCollection*, each addition fires the *CollectionChanged* event, which can be resource-consuming and make the UI very slow.  Instead, the event firing can be frozen, the collection filled, and then event firing is unfrozen again, firing the *CollectionChanged* event only once.
