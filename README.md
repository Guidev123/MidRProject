<p align="center">
  <a href="https://dotnet.microsoft.com/" target="_blank">
    <img src="https://upload.wikimedia.org/wikipedia/commons/e/ee/.NET_Core_Logo.svg" width="120" alt=".NET Logo" />
  </a>
</p>

<h1 align="center">MidR Library</h1>

<p align="center"><strong>MidR</strong> is a lightweight and flexible open-source Mediator library for .NET.</p>

<p align="center">
  <a href="https://www.nuget.org/packages/MidR/">
    <img src="https://img.shields.io/nuget/v/MediR?color=purple&amp;label=NuGet" alt="NuGet version" />
  </a>
  <img src="https://img.shields.io/badge/license-MIT-green.svg" alt="License: MIT" />
  <img src="https://img.shields.io/badge/status-stable-brightgreen" alt="Project Status: Stable" />
</p>

<hr />

<h2>📦 Installation</h2>

<pre><code>Install-Package MediR</code></pre>

<hr />

<h2>⚙️ How to Use</h2>

<h3>🛠️ 1. Manual Dependency Registration</h3>
<pre><code>builder.AddTransient&lt;IAnyHandler, AnyHandler&gt;();</code></pre>

<hr />

<h3>💉 Dependency Injection with MidR</h3>

<h4>2. <code>builder.AddMidR()</code></h4>
<p>
  Uses reflection to scan <strong>all assemblies</strong> in your application for handlers.<br />
  ⚠️ <strong>Less performant</strong>, but convenient for small apps.
</p>
<pre><code>builder.AddMidR();</code></pre>

<hr />

<h4>3. <code>builder.AddMidR(Assembly.GetExecutingAssembly())</code></h4>
<p>Scan only the provided assembly:</p>
<pre><code>builder.AddMidR(Assembly.GetExecutingAssembly());
// or
builder.AddMidR(typeof(AnyCommand).Assembly);</code></pre>

<hr />

<h4>4. Load Specific Assemblies (💨 Most Performant)</h4>
<p>Only load and scan the required assemblies manually:</p>
<pre><code>private static readonly string[] _assemblies =
[
    "SalesSystem.Payments.Application",
    "SalesSystem.Sales.Application",
    "SalesSystem.Catalog.Application",
    "SalesSystem.Registers.Application"
];

foreach (var assembly in _assemblies)
{
    Assembly.Load(assembly);
}

builder.Services.AddMidR(_assemblies);</code></pre>

<hr />

<h2>🧪 Example Project</h2>
<p>
  Check out a full Clean Architecture example using MidR here:<br />
  🔗 <a href="https://github.com/Guidev123/ModularMonolith-EDA" target="_blank">ModularMonolith-EDA</a>
</p>

<hr />

<h2>📝 License</h2>
<p>
  Licensed under the <a href="LICENSE">MIT License</a>.
</p>
