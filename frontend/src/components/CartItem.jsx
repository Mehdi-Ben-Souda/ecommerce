/* eslint-disable react/prop-types */
import { Trash2, Package2 } from "lucide-react";
import QuantityControl from "./QuantityControl";

const CartItem = ({ item, onRemove, onUpdateQuantity }) => {
  return (
    <div className="card bg-base-100 shadow-xl hover:shadow-2xl transition-all duration-300 group">
      <div className="card-body p-8">
        <div className="flex flex-col sm:flex-row gap-8">
          {/* Image Container */}
          <div className="flex-shrink-0 relative">
            <div className="relative overflow-hidden rounded-2xl">
              {item.product.image ? (
                <img
                  src={item.product.image}
                  alt={item.product.name}
                  width={160}
                  height={160}
                  className="w-40 h-40 object-cover rounded-2xl"
                />
              ) : (
                <div className="w-40 h-40 bg-base-200 rounded-2xl flex items-center justify-center">
                  <Package2 size={48} className="text-base-300" />
                </div>
              )}
            </div>
            <div className="absolute top-2 left-2">
              <span className="badge badge-success gap-1">In Stock</span>
            </div>
          </div>

          {/* Content Container */}
          <div className="flex-grow space-y-6 py-2">
            {/* Header */}
            <div className="flex justify-between items-start">
              <div className="space-y-3">
                <h2 className="text-2xl font-bold text-base-content">
                  {item.product.name}
                </h2>
                <p className="text-base-content/70 text-base leading-relaxed max-w-xl">
                  {item.product.description}
                </p>
                <div className="flex gap-3 items-center pt-2">
                  <span className="badge badge-outline">
                    {item.product.category || "Electronics"}
                  </span>
                  <span className="badge badge-primary badge-outline">
                    Free Shipping
                  </span>
                </div>
              </div>
              <button
                className="btn btn-ghost btn-circle hover:bg-error/10 hover:text-error transition-colors duration-300"
                onClick={() => onRemove(item.id)}>
                <Trash2 size={20} />
              </button>
            </div>

            {/* Price and Quantity Section */}
            <div className="flex flex-wrap items-center justify-between gap-6 pt-4">
              <div className="space-y-2">
                <div className="text-3xl font-bold text-primary">
                  ${(item.product.price * item.quantity).toFixed(2)}
                </div>
                <div className="text-base-content/60 text-sm">
                  ${item.product.price.toFixed(2)} per unit
                </div>

                <div className="flex items-center gap-8">
                  <span className="text-base text-base-content/80 font-medium">
                    Quantity:
                  </span>
                  <QuantityControl
                    quantity={item.quantity}
                    onIncrement={() => onUpdateQuantity(item.id, "increment")}
                    onDecrement={() => onUpdateQuantity(item.id, "decrement")}
                  />
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default CartItem;
