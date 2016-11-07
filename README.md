Primitive obsession
==============

Le configurazioni sono dipendenze: vantaggi nell'avere la configurazione nel punto di ingresso dell'applicazione. Le classi non si rompono a runtime, la dipendenza dalla configurazione diventa esplicita, non è possibile istanziare una classe senza fornire la configurazione. La classe diventa indipendente dalla sorgente della configurazione (file? nei test è programmatica)
Quando capita di dover registrare dei primitivi: principalmente per delle configurazioni
public class Foo { }
In quei casi la registrazione diventa prolissa. Portare degli esempi
Trucco con Autofac withParameter. Svantaggio: la registrazione diventa dipendente dal nome del parametro.
Soluzione non buona: iniettare una classe che incapsula la configurazione.
Tuttavia, questo è un ServiceLocator in disguise. Citare l'articolo di Seeman.
Alternativa: raggruppare le configurazioni (esempio: username e password).
Soluzione più generale: Evitare i primitivi. Link a primitive obsession. URL non è una stringa. URL, database connection string.
Trucco in C# per trattare una classe come primitiva.