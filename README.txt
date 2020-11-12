Paso a paso para la ejecución de las pruebas.

Base de datos.
	1.	Buscar el archivo appsettings.Development dentro de la carpeta 1.Aplication y modificar la cadena de conexión por la una instancia de BD donde se desee guardar los registros del parqueadero.
	2.	Seguido a esto, abrir la consola de PackageManagerConsola, seleccionar el proyecto 3.Infraestructure/Infraestructure.Core y ejecutar el comando update-database
	3.	Esos dos pasos son suficientes para la creación de la instancia de la BD de RobinFood.Parqueadero.

PostMan
A continuación se recomienda ejecutar la colección “General” dentro de postman, en el orden que están enumeradas, esto con el fin de realizar la configuración de los valores que se van a cobrar a cada tipo de vehículo cuando se registran las entradas o salidas, si dicha configuración no se tiene creada, aunque se pueden genera ingresos de vehículos al sistema y al parqueadero, no se podrán registrar salidas debido a que no se tiene claro cuando debe pagar cada vehículo por su estancia.
Enumeración de tipos de vehículos
A continuación, se detalla, según la enumeración creada dentro del sistema de parqueadero, que numero corresponde a qué tipo de vehículo
	•	Vehículo no tipificado= 0
	•	Vehículo VIP = 1
	•	Vehículo residente = 2
	•	Vehículo residente pospago = 3
	•	Vehículo no residente = 4
Esta información es valiosa, por si se desean generar nuevas configuración o nuevos valores de cobro para un tipo de vehículo en especial diferente a los que se tienen creados actualmente.
NOTA: La aplicación permite el ingreso de configuración sin importar el tipo de vehículos, y cada que se va a registrar la salida de un vehículo, se valida que tipo de vehículo esta saliendo y se toma el ultimo registro que se tenga configurado para saber el valor con el cual se debe hacer el cobro.


Creación de vehículos:
Para la creación de vehículos, solo basta con ingresar una placa y el tipo de vehículo que se quiere registrar teniendo en cuenta la enumeración que se detallo en el paso inmediatamente anterior.
Solo se permite el registro de vehículos de tipo VIP, Residentes y residentes Pospago.
Se valida que la placa de un vehículo no se halla ingresado anteriormente
Se valida que el vehículo que se valla a ingresar, no este ingresado dentro del estacionamiento (Tenga una entrada vigente)

Registro de entradas:
Para registrar una entrada de un vehículo, basta solo con Ingresar la placa del mismo
	•	El sistema valida que el vehículo no tenga una entrada abierta
	•	El sistema registra la entrada correspondiente


Registro de salidas: 
Para registrar una salida de un vehículo, basta solo con ingresar la placa del mismo
	•	El sistema valida que el vehículo tenga una entrada vigente
	•	El sistema valida el tipo de vehículo y devuelve el valor a cobrar según la siguiente configuración
		o	Vehículo VIP: Devuelve el resultado de la multiplicación de la cantidad de minutos que estuvo el vehículo estacionado por el valor que tiene la configuración para los vehículos de tipo VIP. Adicionalmente, registra la cantidad de tiempo que el vehículo paso dentro del parqueadero para la generación del reporte. NOTA Inicialmente, la configuración de los vehículos VIP se tiene en valor de 0 pesos por minuto para que no haga los cobros como lo indica el caso de uso.
		o	Vehículo Residente y residente Pospago: El registro de salida devuelve un valor a pagar equivalente a 0 pesos, pero registrar el tiempo de la estancia de cada vehículo
		o	Vehículo No residente: El registro de salida, busca el valor configurado para este tipo de vehículo e indica cual es el total a pagar según la cantidad de tiempo de la estancia en el parqueadero


Pagos de residentes:
Para el pago de los residentes y los residentes pospago solo es necesario el ingreso de la placa del vehículo y el sistema hace las siguientes validaciones
	•	Verifica que la placa del vehículo este registrada en el sistema y que sea de tipo Residente o residente pospago
	•	Luego de identificar el vehículo, valida si tiene pagos pendientes para realizar.
	•	En caso de que el vehículo deba pagar algún valor, el servicio devuelve cual es el total que debe pagar el vehículo
	•	NOTA: Este paso se realiza para que el usuario pueda saber si es momento de cobrarle al vehículo y cual es el valor a cobrar. Para ingresar el pago es necesario ejecutar al siguiente paso.


Inserción de pago residentes: 
El sistema valida cuanto debe pagar el vehículo según su tipo y realiza las siguientes validaciones
	•	Valida que el valor del pago ingresado sea mayor a la deuda del vehículo
	•	Valida que el valor del pago ingresado no sea de 0 pesos
	•	Valida que el vehículo tenga deuda pendiente
	•	Si las validaciones anteriores son correctas, realiza el registro del pago del vehículo, y para los vehículos residentes pospago, reinicia la cantidad de minutos de estancia para que pueda pagar el próximo mes.
