using System.Diagnostics;
using DigitalMagazineStore.Models;
using DigitalMagazineStore.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
namespace DigitalMagazineStore.Controllers
{
public class HomeController : Controller
{
private readonly ILogger&lt;HomeController&gt; _logger;
private readonly IHomeRepository _homeRepository;
public HomeController(ILogger&lt;HomeController&gt; logger , IHomeRepository homeRepository)
{
_homeRepository=homeRepository;
_logger = logger;
}
public async Task&lt;IActionResult&gt; Index( string sterm=&quot;&quot;, int categoryID=0)
{
IEnumerable&lt;Magazine&gt; magazines= await _homeRepository.GetMagazines(sterm, categoryID);
IEnumerable&lt;Category&gt; categories= await _homeRepository.Categories();
MagazineDisplay magazineModel = new MagazineDisplay
{
Magazines= magazines,
Categories= categories ,
STerm = sterm,
CategoryID=categoryID
};
return View(magazineModel);
}
public IActionResult Privacy()
{
return View();
}
[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
public IActionResult Error()
{
return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}
}
}