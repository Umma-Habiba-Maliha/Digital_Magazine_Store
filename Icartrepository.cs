using DigitalMagazineStore.Models.DTOs;
namespace DigitalMagazineStore.Repositories
{
public interface ICartRepository
{
Task&lt;int&gt; AddItem(int magazineId, int qty);
Task&lt;int&gt; RemoveItem(int magazineId);
Task&lt;Cart&gt; GetUserCart();
Task&lt;int&gt; GetCartItemCount(string userId = &quot;&quot;);
Task&lt;Cart&gt; GetCart(string userId);
Task&lt;bool&gt; DoCheckout(CheckoutModel model);
}
}