using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace DigitalMagazineStore.Repositories
{
public class CartRepository : ICartRepository
{
private readonly ApplicationDbContext _db;
private readonly UserManager&lt;IdentityUser&gt; _userManager;
private readonly IHttpContextAccessor _httpcontextAccessor;
public CartRepository(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor,
UserManager&lt;IdentityUser&gt; userManager)
{
_db = db;
_userManager = userManager;
_httpcontextAccessor = httpContextAccessor;
}
public async Task&lt;int&gt; AddItem(int magazineId, int qty)
{
string userId = GetUserId();
// Check if the user ID is null at the beginning
if (string.IsNullOrEmpty(userId))
return 0; // Return 0 or a negative value to indicate failure
try
{
// Use an existing or create a new cart
var shoppingcart = await GetCart(userId);
if (shoppingcart is null)
{
shoppingcart = new Cart { UserId = userId };
_db.Carts.Add(shoppingcart);
await _db.SaveChangesAsync(); // Save the new cart to get its Id
}
// Find existing item in the cart or create a new one
var cartItem = await _db.CartDetails
.FirstOrDefaultAsync(a =&gt; a.CartId == shoppingcart.Id &amp;&amp; a.MagazineId == magazineId);
if (cartItem is not null)
{
cartItem.Quantity += qty;
}
else
{
cartItem = new CartDetail
{
MagazineId = magazineId,
CartId = shoppingcart.Id,
Quantity = qty
};
_db.CartDetails.Add(cartItem);
}

await _db.SaveChangesAsync();
}
catch (Exception ex)
{
// Log the exception to understand what&#39;s going wrong
Console.WriteLine(ex.Message);
return 0; // Return 0 to indicate failure
}
// After a successful operation, return the updated cart count
return await GetCartItemCount(userId);
}
public async Task&lt;int&gt; RemoveItem(int magazineId)
{
// using var transaction = _db.Database.BeginTransaction();
string userId = GetUserId();
try
{
if (string.IsNullOrEmpty(userId))
throw new Exception(&quot;User is not logged-in&quot;);
var shoppingcart = await GetCart(userId);
if (shoppingcart is null)
throw new Exception(&quot;Invalid cart&quot;);
//cart detail section
var cartItem = _db.CartDetails
.FirstOrDefault(a =&gt; a.CartId == shoppingcart.Id &amp;&amp; a.MagazineId == magazineId);
if (cartItem is null)
throw new Exception(&quot;No items in cart&quot;);
else if (cartItem.Quantity == 1)
_db.CartDetails.Remove(cartItem);
else
cartItem.Quantity = cartItem.Quantity - 1;
_db.SaveChanges();
}
catch (Exception ex)
{
}
var cartItemCount = await GetCartItemCount(userId);
return cartItemCount;
}
public async Task&lt;Cart&gt; GetUserCart()
{
var userId= GetUserId();
if (userId == null)
throw new Exception(&quot;Invalid userId&quot;);
var Cart = await _db.Carts

.Include(c =&gt; c.CartDetails)
.ThenInclude(cd =&gt; cd.Magazine)
.ThenInclude(m =&gt; m.Category)
.Where(c =&gt; c.UserId == userId).FirstOrDefaultAsync();
return Cart;
}

public async Task&lt;Cart&gt; GetCart(string userId)
{
var shoppingcart= await _db.Carts.FirstOrDefaultAsync(c =&gt; c.UserId == userId);
return shoppingcart;
}
public async Task&lt;int&gt; GetCartItemCount(string userId = &quot;&quot;)
{
// Make sure to get the user ID first
if (string.IsNullOrEmpty(userId))
{
userId = GetUserId();
}
// Now, filter the query by the user ID
var data = await (from shoppingcart in _db.Carts
join cartDetail in _db.CartDetails
on shoppingcart.Id equals cartDetail.CartId
where shoppingcart.UserId == userId // This is the crucial line to add
select new { cartDetail.Id }
).ToListAsync();
return data.Count;
}
private string GetUserId()
{
var principal = _httpcontextAccessor.HttpContext.User;
string userId = _userManager.GetUserId(principal);
return userId;
}
}
}