/* eslint-disable react/prop-types */
// eslint-disable-next-line react/prop-types
import { Plus, Minus } from "lucide-react";

const QuantityControl = ({ quantity, onIncrement, onDecrement }) => {
  return (
    <div className="join border rounded-lg">
      <button
        className="join-item btn btn-sm btn-ghost"
        onClick={onDecrement}
        disabled={quantity <= 1}>
        <Minus size={16} />
      </button>
      <div className="join-item btn btn-sm btn-ghost no-animation pointer-events-none">
        {quantity}
      </div>
      <button className="join-item btn btn-sm btn-ghost" onClick={onIncrement}>
        <Plus size={16} />
      </button>
    </div>
  );
};

export default QuantityControl;
