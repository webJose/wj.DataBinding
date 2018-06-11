# wj.DataBinding

## English

Welcome.  This is a small .Net class library project that provides an assorted set of classes that are meant to help in data binding scenarios.

* **NotifyPropertyChanged**:  This is an implementation of the *INotifyPropertyChanged* interface.  It provides a simple function that can be used to both save and notify of property changes.  The function returns a Boolean value indicating if change occurred, which can be used to further chain notifications of, say, calculated properties.
* **Container**:  It is a generic class that can extend another for the purposes of data binding.  Any properties added to classes that derive from this class are grouped together with the properties of a contained object.  Then the entire set of properties are presented together as one to data-bound controls such as the Windows Forms *DataGridView* control or the WPF *DataGrid* control.
* **ObservableCollectionEx**:  This is an enhanced *ObservableCollection* class.  It allows deactivation (called freezing) of the collection changed event, and later reactivation (unfreezing) with automatic *CollectionChanged* event firing.  It is useful in several scenarios, such as when a control is bound to a collection and the collection then receives many elements.  In the stock *ObservableCollection*, each addition fires the *CollectionChanged* event, which can be resource-consuming and make the UI very slow.  Instead, the event firing can be frozen, the collection filled, and then event firing is unfrozen again, firing the *CollectionChanged* event only once.

## Español

Bienvenid@.  Este es un pequeño proyecto de .Net de biblioteca de clases que provee un conjunto variado de clases cuyo propósito es ayudar en escenarios de vinculación a datos.

* **NotifyPropertyChanged**:  Esta clase es una implementación de la interfaz *INotifyPropertyChanged*.  Provee una simple función que puede usarse para guardar y notificar acerca de cambios de propiedades.  La función retorna un valor Booleano que indica si ocurrió un cambio, que puede usarse para crear una notificación en cadena de, por ejemplo, propiedades calculadas.
* **Container**:  Esta es una clase genérica que puede extender otra para los propósitos de vinculación a datos.  Cualquier propiedad agregada a clases que derivan de esta clase son agrupadas con las propiedades del objeto contenido.  Este conjunto completo de propiedades es entonces presentado como uno a controles que vinculan a datos como por ejemplo el control *DataGridView* de Windows Forms o el control *DataGrid* de WPF.
* **ObservableCollectionEx**:  Esta clase es una versión mejorada de *ObservableCollection*.  Permite la desactivación (llamado congelamiento) del evento de cambio de colección, y posterior reactivación (descongelamiento) con disparo automático del evento *CollectionChange*.  Es útil en varios escenarios, como cuando un control es vinculado a una colección y la colección luego recibe muchos elementos.  En la clase estándar *ObservableCollection*, cada adición desencadena el evento *CollectionChanged*, cosa que puede consumir muchos recursos y tornar la interfaz gráfica (UI) muy lenta.  En su lugar, el desencadenamiento del evento puede congelarse, luego llenar la colección, y luego descongelar el desencadenamiento del evento, lo que produce un único desencadenamiento de *CollectionChanged*.
