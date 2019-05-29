
<html>
<head>
<title>Article Source</title>
<link rel="stylesheet" type="text/css" 
    href="//codeproject.cachefly.net/App_Themes/CodeProject/Css/Main.css?dt=2.8.151108.1" />
<base href="http://www.codeproject.com/KB/cs/" />
</head>
<body>
<!--
HTML for article "Using a Plugin-Based Application" by Jean-Pierre Bachmann
URL: http://www.codeproject.com/KB/cs/682642.aspx
Copyright 2013 by Jean-Pierre Bachmann
All formatting, additions and alterations Copyright © CodeProject, 1999-2015
-->


<div>
<!-- Start Article -->
<span id="ArticleContent">

<p>There is a small Tutorial for this topic on <a href="https://www.youtube.com/watch?v=pvXi1lbLz-s">YouTube</a>&nbsp;</p>

<p style="text-align: center;"><em style="color: rgb(51, 51, 51); font-size: 30px;"><strong>HINT !</strong></em></p>

<p>Please read the article very carefully because some guys tried to use it and did not understand how it works. For example: in some cases you are forced to setup VS to recompile all DLL&#39;s on every build! that is imported because due the loose of Dependency between the start project and the Module containing DLL VS does not recognize that the DLL has changed so it would maybe not build it again. To enable this behavior go to TOOLS - PROJECTS AND SOLUTIONS -&gt; BUILD AND RUN and set the option &quot;On Run, When projects are out of date&quot; To &quot;Always Build&quot;. (Visual studio restart required)</p>

<h3>Required knowledge</h3>

<p>I expect you to know your language. You should be aware of class inheritance, some basic understanding of Inversion of Control and Dependency injection.</p>

<h2>Repository</h2>

<p>The Repository contains 4 Branches.</p>

<p>The Trunk:</p>

<p style="margin-removed 40px;"><em>JPB.Shell.exe </em></p>

<ul>
	<li>
	<p style="margin-removed 40px;"><em>Contracts.DLL </em></p>

	<ul>
		<li>
		<p style="margin-removed 40px;"><em>Shell Interfaces and Attributes host</em></p>
		</li>
	</ul>
	</li>
	<li>
	<p style="margin-removed 40px;"><em>MEF.DLL </em></p>

	<ul>
		<li>
		<p style="margin-removed 40px;"><em>The Framework itself</em></p>
		</li>
	</ul>
	</li>
</ul>

<p>and 3 Examples</p>

<p>They all depends on the Main Nuget Package.</p>

<h4>Console:</h4>

<ul>
	<li><em>JPB.Shell.Example.</em>

	<ul>
		<li>Console.exe
		<ul>
			<li><i>Start application</i></li>
		</ul>
		</li>
		<li>BasicMathFunctions.DLL
		<ul>
			<li>Implementation for a Simple calculator</li>
		</ul>
		</li>
		<li>Contracts.DLL
		<ul>
			<li>The Application specify Interfaces</li>
		</ul>
		</li>
	</ul>
	</li>
</ul>

<p>The Console application should help you to understand how a Plugin based application should be designed and how in general the framework could be used without any UI specific relevation</p>

<h4>Foo:</h4>

<p>Contains:</p>

<ul>
	<li>JPB.Shell.
	<ul>
		<li>exe
		<ul>
			<li>Contains the Loading mechanism but not more</li>
		</ul>
		</li>
		<li>CommonApplicationConatiner.DLL
		<ul>
			<li>Contains the a Generic Ribbon UI that loads the other services</li>
		</ul>
		</li>
		<li>CommonContracts
		<ul>
			<li>For interfaces only ... this extends IService interfaces to extend the functionality</li>
		</ul>
		</li>
		<li>VisualServiceScheduler
		<ul>
			<li>A WPF MVVM Module that provides a Surface that can be used in any other app without rebuilding</li>
		</ul>
		</li>
	</ul>
	</li>
	<li>JPB.Foo.
	<ul>
		<li>Client.Module.DLL
		<ul>
			<li>Very simple Visual Module</li>
		</ul>
		</li>
		<li>ClientCommenContracts
		<ul>
			<li>The extension of JPB.Shell.Contracts</li>
		</ul>
		</li>
		<li>CommenAppParts
		<ul>
			<li>Single sample implementation of the extended Interfaces</li>
		</ul>
		</li>
	</ul>
	</li>
</ul>

<p>This Solution should show you a sample application that contains how to build a complete Plugin based application for WPF. The basic app is the same as in some Web project or Forms. You provide the Modules via MSEF and then load them by accessing there properties.</p>

<h3>Introduction</h3>

<p>Building, Maintaining and the actual usage of a plugin based application is something new to a lot of us Developer. But in our world of fast changing Requirements and Open-Source projects around us all, its something that will cross the way of almost every Dev in this career. Even if this just a moment in a project when we are thinking &quot;hmm this will change in future lets use an interface to ensure that the work will be not that big&quot;. For that case i will introduce you a method to get this approach in a new level. Lets talk about some Buzzwords as <em>Dependency Injection </em>and <em>Inverse of Control (IoC). </em>There some nice Articles here on CodeProject so i will not explain them in deep just to ensure we are talking about the same thing. Dependency injection means that we move the <em>responsibility</em> of certain function from the actual actor away. We Inject some logic into a class that does not know what exactly happen. The only constraint to this approach is that at the most times the target Actor must support this kind of behavior. And at least if the class is not intent to allow this coding pattern you should not use it. Most likely you would break the desired code infrastructure if you try to take control over some process when the process is not designed to allow this. Inverse of Control is like the Dependency Injection a coding pattern and is intended to give a process from one actor to an other. As the name say, it allows the Control over a process from the original Actor to some other. This apples to the simple usage of an Delegate for Factory creating as on the Complete WPF Framework. We give away the Control about <em>how </em>something is done.</p>

<h3>Background</h3>

<p>The reason why this framework as created by me was very simple: I was facing some problems with the creation of a program that was developed fast but then, as a Big surprise for me and the Stakeholder was grown as Hell.</p>

<p>At the end we had a Program that was so big that is was nearly unable to maintain. We decided to redevelop and as we had so many different&nbsp;functions inside this application ( what was not desired to contain all this functions ) we also decided to allow this kind of function &quot;madness&quot;.</p>

<p>Then the idea of some kind of Plugin/Module driven application was born inside my head. I&#39;d made some research and found the <a href="https://msdn.microsoft.com/en-us/library/dd460648(v=vs.110).aspx">Managed Extensibility Framework (MEF)</a> inside the .net Framework. But as fast I&#39;d found this, i found the limitations of it. In my opinion it was just to Slow and for this kind of &quot;simple&quot; work. I started extending MEF and wrote some manager classes. The result of my efforts you can explorer here today. As it started for UI related application we fast discovered the full advantages of my code. The code is simple as useful and does 2 things. First it speeds up the enumeration process with the usage of Plinq and some other Optimisations. Second&nbsp;it limit&#39;s the Access. Why would this be a good thing you may ask yourself now. MEF provides you a lot of &quot;extra&quot; functionality and some kind of &quot;Nuclear Arsenal&quot; of Configuration. This is for a developer that has no idea of Dependency injection and IoC far to much. So Limiting the Configuration and hide some things you do not necessary need to take care of, is the exact right thing. Some configuration was just changed into some more centralized way.</p>

<h4>MSEF</h4>

<p>The Managed Service Extensibility Framework (MSEF) builds up on MEF and extents the usage and Access to MEF. It wraps all MEF Exports into services that can be accessed through its Methods. A class represents one or more Services. When a class defines one or more Services it must not implement its functionality. In the Worst case scenario this will break the law of OOP.</p>

<h4>Structure</h4>

<p>Every shared code we want to manage must be wrapped into an Assembly. This Assembly contains classes that inherits from <code>IService</code>. IService is most likely of an Marker interface with just one definition of an starting Method that will be invoked when the Service started once. Every service that will be accessed by the Framework will be handled as SingleInstance. So Technically there should be just one instance per Service definition. You can create your own instances by using new(), but they will not be observed by the MSEF.</p>

<pre lang="cs">
namespace JPB.Shell.Contracts.Interfaces.Services
{
    public interface IService
    {
        void OnStart(IApplicationContext application);
    }
}</pre>

<p>The IService Interface it the 1st Important thing when we talk about the Implementation. But searching for inheritance would be slow. So we need something additional like an Attribute. An .net Attribute is the desired way of adding meta data to a class. The base Attribute is the <code>ServiceExportAttribute</code></p>

<pre lang="cs">
public class ServiceExportAttribute : ExportAttribute, IServiceMetadata
</pre>

<div>as you see it also Inherits from the MEF Export attribute and in addition to the MSEF interface. The base class ensures a enumeration from MEF <em>so all classes that are marked for Export are also usable from MEF without change</em>. The <code>IServiceMetadata </code>attribute contains some Additional Infos.</div>

<pre lang="cs">
public interface IServiceMetadata
{
    Type[] Contracts { get; }
    string Descriptor { get; }
    bool IsDefauldService { get; }
    bool ForceSynchronism { get; }
    int Priority { get; }
}</pre>

<p>As i said a class can be used to define one or more Services. To maintain this information to the outside without analyzing the class self the Information must be written to the Attribute. For that the Contracts array is used. For every type in this collection the defined class will be seen as the type.</p>

<p>Every aspect of this code is extendable. If the code does not fit into your need just extend it or wrap it. Just this basic information must be provided.&nbsp;</p>

<p>This Array of implemented interfaces has pro&#39;s and con&#39;s. On the Pro site its fast as hell to not load the class from the assembly and then analyze it. But this is also a big disadvantage. You have to define your service &quot;declaration&quot; twice. In the MetaData and also in your actual class. When both are not match there are 2 possible scenarios. You implement a class in your code but not define it in your MetaData. This is not so bad because the worst thing is that the MSEF will not find it. Not good but not the end of the world. The more dangerous thing is that you define a interface in your MetaData but not implement it! Every compiler would not throw an error if you declare an interface but not actually implement it. In this case the code would compile but it will throw an exception on runtime what is, actually pretty bad. I&#39;m planning an <a href="https://roslyn.codeplex.com/" style="color: rgb(102, 0, 153); removed: pointer;">Roslyn</a>&nbsp;extension to take care of these problems but this may take a while.</p>

<h3>Using the code</h3>

<p>The normal usage would be the following:</p>

<p>You define some sort of service by creating Interfaces that inherits from <code>IService</code>.</p>

<pre lang="cs">
public interface IFooDatabaseService : IService
{
    //CRUD
    void Insert&lt;T&gt;(T entity);
    T Select&lt;T&gt;(string where);
    void Update&lt;T&gt;(T entity);
    bool Delete&lt;T&gt;(T entity);
}</pre>

<p>That is your service definition. You could just extend this interface as much you want even inheriting from other Interfaces or extend this Service by another service. That&#39;s all possible as long as it inherits from <code>IService</code>. To make the service usable you must implement it in some DLL and mark it with the <code>ServiceExport </code>Attribute.</p>

<pre lang="cs">
[ServiceExport(
    descriptor: &quot;Some name&quot;,
    contract: new[] 
    { 
        typeof(IFooDatabaseService),
        //Additional Services like services that are inherit 
    }
    )]
class FooDatabaseService : IFooDatabaseService
{
    public void Insert&lt;T&gt;(T entity)
    {
        throw new NotImplementedException();
    }
 
    public T Select&lt;T&gt;(string where)
    {
        throw new NotImplementedException();
    }
 
    public void Update&lt;T&gt;(T entity)
    {
        throw new NotImplementedException();
    }
 
    public bool Delete&lt;T&gt;(T entity)
    {
        throw new NotImplementedException();
    }
 
    public void OnStart(IApplicationContext application)
    {
        throw new NotImplementedException();
    }
}</pre>

<p>You may recognized&nbsp;that the class had no access modifier so its private by default. This is possible but really bad. As MEF does not more then using Reflection it can and will break the law of OOP. So keep in mind that these laws are not applied here and only enforced due compile time.</p>

<p>We now created all necessary things. We extended the IService interface to allow our own service implementation and marked the class as an Service with the <code>ExportService </code>attribute. Next step would be the enumeration of services and usage of that service.</p>

<p>To start the initial process we must first create the processor. The process is handled be the ServicePool class.</p>

<pre lang="cs">
[DebuggerStepThrough]
public class ServicePool : IServicePool
{
    internal ServicePool(string priorityKey, string[] sublookuppaths) 
}</pre>

<p>It only contains a internal constructor and can only created with the ServicePoolFactory.</p>

<pre lang="cs">
namespace JPB.Shell.MEF.Factorys
{
    public class ServicePoolFactory
    {
        public static IServicePool CreatePool()
        {
            return CreatePool(string.Empty, Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
        }
 
        public static IServicePool CreatePool(string priorityKey, params string[] sublookuppaths)
        {
            var pool = new ServicePool(priorityKey, sublookuppaths);
            ServicePool.Instance = pool;
            if (ServicePool.ApplicationContainer == null)
                ServicePool.ApplicationContainer = new ApplicationContext(ImportPool.Instance, MessageBroker.Instance, pool, null, VisualModuleManager.Instance);
            pool.InitLoading();
            return pool;
        }
 
        public static async Task&lt;IServicePool&gt; CreatePoolAsync(string priorityKey, params string[] sublookuppaths)
        {
            return await new Task&lt;IServicePool&gt;(() =&gt; CreatePool(priorityKey, sublookuppaths));
        }
    }
}</pre>

<p>This factory takes care of all internal properties and the initial call of <code>InitLoading.</code></p>

<p>&nbsp;</p>

<p>Another way would be to access the static <code>ServicePool.Instance</code> property what will do the same as calling CreatePool.</p>

<p>The Service pool will start the Enumeration of all files in the given path by using the priority key to define Assemblies that will be searched with level 1 priority. These files are handled as &quot;necessary&quot; for your program. They should contain all basic logic like the Window in a Visual application or other starting procedures (later more to that). The process will be done as the following:</p>

<p>Assume:</p>

<p>PriorityKey = &quot;*Module*&quot;</p>

<ol>
	<li>Enumerate all DLLs in your shell directory</li>
	<li>Search for a specific part in the name and Flag them as high priority of the name to improve Performance (That just means that DLLs with a name like &quot;FooClientModule.DLL&quot; are loaded at Startup but DLLs like &quot;FooClient.DLL&quot; not! )</li>
	<li>Search those high priority assemblies for exports and add them into my <code>StrongNameCatalog</code>, skip all non high priority assemblies</li>
	<li>Wait for ending of 3</li>
	<li>Start the default service <code>IApplicationContainer</code></li>
	<li>The main window is opening</li>
	<li>All assemblies without the high priority flag will be searched for exports</li>
</ol>

<h3>Handling of <code>IApplicationContainer</code>.</h3>

<p>This interfaces is provided by the Framework self and indicates a service that must be started as it&#39;s available. If the enumeration process is done this service will be instantiated. For this process the ServiceExport Attribute contains the optional parameter <code>ForceSyncronism</code> and <code>Priority</code>. If a <code>IApplicationProvider</code> does not provide this Information <code>ForceSyncronism</code> will be <code>False </code>and the Priority will be very Low. The first parameter is very important because it will cause to block your caller as long as there are Services that are not executed.</p>

<ol>
	<li>Enumerate all <code>IApplicationProvider</code> that are marked to executed Async and start them Unobserved</li>
	<li>Execute all services that are marked to be executed synchrony inside the caller thread</li>
</ol>

<p>This info is important when using multiple Applications that are depend on each other. When ApplicationSerivce A tries to load data from ApplicationService B but service B is not loaded it will fail and cause in Strange behaviors then<em> remember just because it will work on your machine does not mean that it will work on every maschine.</em> This is caused be the huge impact of Multithreadding and Tasking. Every Machine decides on its own how Tasks and Threads are handled.</p>

<h3>Usage and Meaning of IApplicationProvider.</h3>

<p>The Idea is simple. As far as we have a application that is only connected with loosely dependencies over Interfaces, there is no Starting mechanism too. To support this kind of unobserved starting from the caller, we use this interface. Useful implementations would be Service that pulls data from a Database that must be available at start. So when we think back to our <code>FooDatabaseService</code>, it provides us a method to access a Database and Preload the request Data.</p>

<p>But this brings us too the next problem is a service based application. The communication between services is a very complex point. As long as we could not really expect a service to be exist, the framework brings its own. This kind of communication is implemented inside the IService interface.</p>

<pre lang="cs">
public interface IService
{
    void OnStart(IApplicationContext application);
}</pre>

<p>Every service contains this starting logic and handles the IApplicationContext. This interfaces provides us the basic functions like DataStorage(DataBroker), DataCommunication(MessageBroker), ServiceHandling(ServicePool) and some more. The idea is that services are Isolated from each other and from the Main logic of your application. They can not know you and you don&#39;t know them. So the Framework provides a way of communication</p>

<pre lang="cs">
public interface IApplicationContext
{
    IDataBroker DataBroker { get; set; }
    IServicePool ServicePool { get; set; }
    IMessageBroker MessageBroker { get; set; }
    IImportPool ImportPool { get; set; }
    IVisualModuleManager VisualModuleManager { get; set; }
}</pre>

<p><code>IDataBroker</code> provides your service a centralized interface for Storing data in a persistent way. In the the current State of Development this is the the only Context Property that is null per default. If you want to provide your services a DataBroker then you need to set it from the outside or from one of your Services. One possible solution would be:</p>

<pre lang="cs">
  [ServiceExport(
      descriptor: &quot;AppDataBroker&quot;,
      isDefauld: false, 
      forceSynchronism: true, 
      priority: 0, 
      contract: new[] { 
          typeof(IDataBroker),
          typeof(IApplicationProvider)
      })]
  class FooDataBroker : IDataBroker, IApplicationProvider
  {
      #region IDataBroker Implementation
      ...
      #endregion
 
      /// &lt;summary&gt;
      /// 
      /// &lt;/summary&gt;
      /// &lt;param name=&quot;application&quot;&gt;&lt;/param&gt;
      void Contracts.Interfaces.Services.IService.OnStart(IApplicationContext application)
      {
          //Do some Initial things like open a database and pull application settings
          ...
          //add yourself as the DataBroker
          application.DataBroker = this;
      }     
  }</pre>

<p>From the top: We define the Export attribute to mark this class as a Service. Force the Synchronism and priority to 0 to load this before all other interfaces ( depends on your application logic may this service depends also on an other ). At least we define 2 interfaces to be exported. First IDataBroker so if some other components will ask for this Interface ( inherits also from IService ) it will get this one, and 2nd IApplicationProvider so that we will be called as soon as Possible.</p>

<p>Next thing is the IMessageBroker that allows us to Transport data from one Costumers to another. It has a Standard implementation that allows everyone to add him self as a Consumer based on a Type as a key. If then some other consumer will publish data that is of type of a key, all Costumers will be notified.</p>

<pre lang="cs">
public interface IMessageBroker
{
    void Publish&lt;T&gt;(T message);
    void AddReceiver&lt;T&gt;(Action&lt;T&gt; callback);
}</pre>

<p>A useful other implementation would be a service that checks T for be some kind of special message and if so it could publish the message instead of locally to a WCF service and spread the message to other Customers. This could extend you&#39;re local application from Plugin based to even Remote Applications.</p>

<p>&nbsp;</p>

<p>So as no one knows the caller, no one would be able to work with other parts of the application. For that general case every service has to know the global ServicePool. To get even rid of this dependency between every Service and the MSEF framework processor, the ServicePool is contained inside the ApplicationContext. With this infrastructure, no service is known of the Caller and we centralized all logic inside the ServicePool. Like the Hollywood principle &quot;Don&#39;t call us, we call you&quot; we got a absolutely clear abstraction of every logic.</p>

<p>But this has also a disadvantage. If no one in your application else then the Original Starter inside the .exe, knows the IServicePool, how could they Query against it? At this point the Developer comes into play. There is no &quot;how it must be done&quot; solution for this problem, just one restriction. To maintain your plugin approach do not reference JPB.Shell or your exe directly and Vice versa.&nbsp;</p>

<blockquote class="quote">
<div class="op">Quote:</div>

<p><span style="color: rgb(128, 128, 128); font-weight: bold;">Store the&nbsp;</span><span style="font-weight: bold;">Contracts.Interfaces.Services.IService.OnStart(IApplicationContext application)</span></p>

<p><span style="color: rgb(17, 17, 17);">That&#39;s simple. For each DLL that contains a Service like your <code>FooDatabaseService</code>&nbsp;create a 2nd Service and call it Module. This service is an <code>IApplicationProvider</code> and stores the <code>IApplicationContext </code>in a Static variable. Because of the fact that the Module service will always be called before every other service, the variable ( let&#39;s call it Context ) will never be null and you always have a valid reference to the MSEF without knowing the caller or the MSEF DLL.</span></p>
</blockquote>

<p>Last but not least we will talk about the most basic part, the direct call of IServicePool. e.g.. How can we obtain a service from the Framework.</p>

<pre lang="cs">
//Module is my VisualModule that is invoked before
           var infoservice = Module
               //Context is my static variable of IApplicationContext
           .Context
               //ServicePool is simply the current instance of ISerivcePool
           .ServicePool
               //GetSingelService gets a singel service of the requested Interface it has the FirstOrDefauld behavior
           .GetSingelService&lt;IFooDatabaseService&gt;();
 
           //Check if null
           if (infoservice == null)
               return;
 
           //For example get the last Entry in the ImportPool that logs every actively that is done be ServicePool
           var logEntry = Module.Context.ImportPool.LogEntries.Last();
 
           //Call a Function on that service that we got
           //in that case we want to insert the Log into the DB
           infoservice.Insert(logEntry); </pre>

<h3>Metadata and Attributes</h3>

<p>There are 2 ways for using Metadata. Based on a Service implementation and based on the service self. Every service can define its own Metadata by using ServiceExport and its Properties or the service ( represented by its Interface ) can define some standard properties that will be applied on all inherited services.</p>

<p><img border="0" hspace="0" src="682642/Service_and_Inherited_export.jpg" vspace="0" /></p>

<p>In this case the service interface does not contain any kind of Metadata, the implementations defines Metadata on there own.</p>

<p><img border="0" hspace="0" src="682642/Service_and_Inherited_export_-_Copy.jpg" vspace="0" /></p>

<p>In this case we inverted the MetaData away from the Implementation to the declaration of the Service.</p>

<p>This offers as the following usage:</p>

<pre lang="cs">
public class FooMessageBox : IFooMessageBox
{
    Contracts.Interfaces.IApplicationContext Context;
 
    public void showMessageBox(string text)
    {
        MessageBox.Show(&quot;Message from Service: &quot; + text);
    }
 
    public void OnStart(Contracts.Interfaces.IApplicationContext application)
    {
        Context = application;
    }
}</pre>

<p>For this case we can skip the MetaData declaration on Class level and move the MetaData to the interface.</p>

<pre lang="cs">
[InheritedServiceExport(&quot;FooExport&quot;, typeof(IFooMessageBox))]
public interface IFooMessageBox : IService
{
    void showMessageBox(string text);
}</pre>

<p>This has good and bad sides:</p>

<p>Good:<br />
We do not need to care about the Metadata because we did that once while we created the Interface.</p>

<p>Bad:</p>

<p>We can not longer control or modify the Metadata. Since the Attribute needs constant values, every implementation of <code>IFooMessageBox</code> provides the same Metadata and for the framework, there all the same. So we got this problem:</p>

<p><img src="682642/Inherited_exportproblem.jpg" /></p>

<p>You can see since we are using the <code>InheritedServiceExportAttribute</code> we don&#39;t know who is who because there all declares the same Metadata.</p>

<h3>Using the code for UI</h3>

<p>There is no deniable advantage for UI application that are using Plugins to extend there surface and logic. Even for this reason the Framework was original designed. I talked a lot how to use the Framework in general so now i will introduce you to the possible usage for a UI Application.</p>

<p>By setting up a Complete new Project you need a starting logic that invokes the Enumeration of the modules and a directory. So even by creating a new ConsoleApplication you need a starting logic. But let us go a bit more Specific. As it was designed for it, the Framework contains a easy to use service for applications that are using the MVVM.</p>

<p>It contains a Interface <code>IVisualService</code> and a Service <code>IVisualModuleManager</code>. Both builds up to support MVVM.</p>

<p>The <code>IVisualModuleManager </code>is implemented by the <code>VisualModuleManager </code>and it is most likely a wrapper for the <code>ServicePool</code> that filters for <code>IVisualService</code>&#39;s.</p>

<p>A Possible usage would be that the executable calls the <code>ServicePool </code>and starts the Process. A DLL contains a <code>IApplicationProvider</code> and he will show a Windows with a List. Then the ApplicationProvider will use the given <code>VisualModuleManager </code>to ask for all Instances of <code>IVisualServiceMetadata</code> and populates the Listbox with it. As we are only Query for the MetaData, the no actual Service is created! This has an extreme effect on Performance. When selecting an item on this list box, the AppProvider will call <code>VisualModuleManager </code>to create a <code>IVisualService </code>based on the given Metadata. Then the other Service will be created and we can access a VisualElement by calling the property View of <code>IVisualService</code>. Last but not least, the <code>VisualServiceManager</code> need some inherited metadata called <code>VisualServiceExport</code>. If you would like to you can extend both the Metadata or the <code>IVisualService </code>to add your own properties or functions.</p>

<p>An proper example with a Ribbon is in Github.</p>

<h3>Before you start</h3>

<p>To ensure that your application and you Models goes into the same folder ( that is very important because how should MEF know where are your DLLs? ;-), set the BuildPath to the same as the Shell ( Default is &quot;..\..\..\..\bin\VS Output\&quot; for Release and &quot;..\..\..\..\bin\VS Debug\&quot; for Debug. Than ensure that your set the &quot;On Run,When Projects out of date&quot; option to Always build. Also Remind yourself that MEF is not designed to load Services from a .exe. So only .DLL &#39;s are supported.&nbsp;</p>

<h3>Features that you should know</h3>

<p>There are 2 <a href="http://msdn.microsoft.com/en-us/library/ed8yd1ha%28v=vs.110%29.aspx"> <em>Preprocessor</em> Directives </a> that controls some of the behavior.</p>

<pre lang="cs">
#define PARALLEL4TW</pre>

<p>Allows the StrongNameCatalog the using of PLinq to search and include the Assemblies into the &not; Catalog with a mix of Eager and Lazy loading. For my opinion you should not disable this in your version because it is Tested, Save and very fast. Just for debugging purposes does this make sense.</p>

<pre lang="cs">
#define SECLOADING</pre>

<p>As in <a href="http://blogs.msdn.com/b/dsplaisted/archive/2010/11/01/how-to-control-who-can-write-extensions- ¬
for-your-mef-application.aspx"> this </a> link described, the Catalog contains a way to guarantee a small amount of security. I never used this so i can not say anything to that.</p>

<h3>Possible Scenario&#39;s</h3>

<p>There are a lot of possible Scenarios. I will explain some to give you some ideas how the system is working and for what kind of work it is designed. We will start with a simple application that doing some work. Like a Calculator it can Multiply numbers. As the System was designed with some Interface logic it contains an interface named <code>ICalculatorTask.</code> This interface defines only one method that is called <code>Calculate()</code>. this Method takes only 2 Parameters of type string. There are now 2 ways to implement this Plugin approach.</p>

<p>1. Reimpliment ICalculatorTask and Inherit from IService</p>

<p>This approach will cause all existing and future tasks the be &quot;known&quot; as a service. This is sometimes not exactly what we want. This would be the easy way and sometimes the best but this depends on your coding style. By Implementing <code>IService </code>and then Defining the export attribute, we could load all interfaces at once and without any dependency to it.</p>

<p>2. Inherit from ICalculatorTask and create an Service Adapter</p>

<p>This approach will take use of the Adapter pattern and will wrap all Service functionally to an own class. That is useful if we don&#39;t want to alter the existing code and prevent the target classes the be known that they are really used as Services. To do this we will create a class called <code>CalculatorWrapper</code>. This base calls will take a <code>ICalculatorTask </code>instance and will delegate all tasks to it. Then we inherit from this wrapper class and create a new class called Multiply or what every exact instance we are defining.</p>

<p>this simple Scenario would be a good idea to use a Plugin based system.</p>

<div>an proper example with a Console and a small Calculator is in Github.</div>

<h3>Points of Interest</h3>

<p>I had a lot of fun while creating this project and I guess it is worth it, to share not only the idea with other developers and everyone interested in it. When I started with MEF in .NET 4.0 I was driven crazy because the pure MEF system is very Complex. I maintain this project now for more then a year and still got ideas how to extend or improve it.</p>

<p>I would highly appreciate any new ideas and impressions from you.</p>

<p>Also i would like to hear / see applications you made with it.</p>

<p>Just Contact me here. Thank you in advice.</p>

<h2>Github</h2>

<p>As suggested from user John Simmons i cleaned up the repository and removed everything WPF specific from the trunk solution. Now i only contains the 2 Main assemblies:</p>

<ol>
	<li>JPB.Shell.MEF</li>
	<li>JPB.Shell.Contracts</li>
</ol>

<h2>Help me!</h2>

<p>As i am very interested in Improving my Skills and the Quality of my code, i created a Form the receive input from you directly. If you are using this project please feel free to take it, it will not least longer then a minute.</p>

<p>https://docs.google.com/forms/d/1LX68Simcv8naq_hOf7jkOSKbT_QysE0jqMSM_appG2s/viewform?usp=send_form</p>

<h3>History</h3>

<p>V1: Initial creation</p>

<p>V1.1: Minor bugfixes as:</p>

<ol>
	<li>Fixed the MEF container bug that causes a IService method OnLoad could be called multiple times</li>
	<li>Fixed the unporpper usage of INotifyPropertyChanged implementation of IServicePool</li>
	<li>Due the first call, all services are wrapped into a new Lazy instance that will take care of OnStart.</li>
</ol>

<p>V2 Minor changes:</p>

<ol>
	<li>Removed unessesary things from the Trunk</li>
	<li>Made a Console example</li>
	<li>Added as Nuget Packet ( See Top )</li>
</ol>

<p>V3 Changes:</p>

<p>Rewrote the Article in CodeProject</p>

<p>New features:</p>

<ol>
	<li>New funktion for loading callbacks inside the IImportPool</li>
</ol>



</span>
<!-- End Article -->




</div> 
</body>
</html>
