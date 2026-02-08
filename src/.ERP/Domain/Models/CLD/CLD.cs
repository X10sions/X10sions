namespace X10sions.ERP.Data.Models.CLD;


/*


"-- Work Orders (Production)
CREATE TABLE work_orders (
    wo_id INT PRIMARY KEY AUTO_INCREMENT,
    wo_number VARCHAR(50) UNIQUE NOT NULL,
    wo_type VARCHAR(50), -- 'PRODUCTION', 'ASSEMBLY', 'REWORK', 'DISASSEMBLY'
    produced_item_id INT NOT NULL, -- Output item
    planned_quantity DECIMAL(15,3) NOT NULL,
    actual_quantity DECIMAL(15,3) DEFAULT 0,
    wo_date DATE NOT NULL,
    planned_completion_date DATE,
    actual_completion_date DATE,
    wo_status VARCHAR(50), -- 'PLANNED', 'RELEASED', 'IN_PROGRESS', 'COMPLETED', 'CANCELLED'
    created_by VARCHAR(100),
    created_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (produced_item_id) REFERENCES stock_items(item_id),
    INDEX idx_wo_number (wo_number),
    INDEX idx_wo_status (wo_status),
    INDEX idx_wo_type (wo_type)
);"
"-- Work Order Outputs (Produced Inventory)
CREATE TABLE wo_outputs (
    wo_output_id BIGINT PRIMARY KEY AUTO_INCREMENT,
    wo_id INT NOT NULL,
    inventory_id BIGINT NOT NULL,
    output_quantity DECIMAL(15,3) NOT NULL,
    output_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (wo_id) REFERENCES work_orders(wo_id),
    FOREIGN KEY (inventory_id) REFERENCES inventory(inventory_id),
    INDEX idx_wo (wo_id),
    INDEX idx_inventory (inventory_id)
);"
"-- Work Order Inputs (Consumed Inventory)
CREATE TABLE wo_inputs (
    wo_input_id BIGINT PRIMARY KEY AUTO_INCREMENT,
    wo_id INT NOT NULL,
    item_id INT NOT NULL,
    required_quantity DECIMAL(15,3) NOT NULL,
    consumed_quantity DECIMAL(15,3) DEFAULT 0,
    FOREIGN KEY (wo_id) REFERENCES work_orders(wo_id),
    FOREIGN KEY (item_id) REFERENCES stock_items(item_id),
    INDEX idx_wo (wo_id),
    INDEX idx_item (item_id)
);"
"-- WO Input to Inventory Link
CREATE TABLE wo_input_inventory_link (
    link_id BIGINT PRIMARY KEY AUTO_INCREMENT,
    wo_id INT NOT NULL,
    wo_input_id BIGINT NOT NULL,
    inventory_id BIGINT NOT NULL,
    consumed_quantity DECIMAL(15,3) NOT NULL,
    consumption_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (wo_id) REFERENCES work_orders(wo_id),
    FOREIGN KEY (wo_input_id) REFERENCES wo_inputs(wo_input_id),
    FOREIGN KEY (inventory_id) REFERENCES inventory(inventory_id),
    INDEX idx_wo (wo_id),
    INDEX idx_inventory (inventory_id)
);"

"-- Current Inventory Summary with All Allocations
CREATE VIEW v_inventory_summary AS
SELECT 
    i.inventory_id,
    i.inventory_number,
    e.entity_name AS owning_entity,
    si.item_code,
    si.item_description,
    i.current_price,
    qsc.status_name AS quality_status,
    i.max_quantity,
    i.planned_quantity,
    i.actual_quantity,
    i.allocated_quantity,
    (i.actual_quantity - i.allocated_quantity) AS available_quantity,
    CONCAT(l.warehouse, ' - ', l.zone, ' - ', l.aisle, ' - ', l.shelf) AS location_full,
    i.lot_number,
    i.serial_number,
    i.entry_date,
    i.expiry_date,
    i.inventory_status,
    
    -- Incoming PO Info
    po.po_number AS incoming_po_number,
    po.invoice_confirmed AS po_invoice_confirmed,
    po.invoice_date AS po_invoice_date,
    
    -- Outgoing SO Info
    so.so_number AS outgoing_so_number,
    so.invoice_confirmed AS so_invoice_confirmed,
    so.invoice_date AS so_invoice_date,
    
    -- Transit Order Info
    tord.to_number AS transit_order_number,
    tord.to_status AS transit_status,
    l_to.location_name AS transit_destination,
    
    -- Work Order Output Info
    wo_out.wo_number AS wo_output_number,
    wo_out.wo_type AS wo_output_type,
    
    -- Work Order Input Info
    wo_in.wo_number AS wo_input_number,
    wo_in.wo_type AS wo_input_type
    
FROM inventory i
INNER JOIN owning_entities e ON i.entity_id = e.entity_id
INNER JOIN stock_items si ON i.item_id = si.item_id
INNER JOIN locations l ON i.location_id = l.location_id
INNER JOIN quality_status_codes qsc ON i.quality_status_id = qsc.status_id

-- Left join for PO
LEFT JOIN po_inventory_link pol ON i.inventory_id = pol.inventory_id
LEFT JOIN purchase_orders po ON pol.po_id = po.po_id

-- Left join for SO
LEFT JOIN so_inventory_link sol ON i.inventory_id = sol.inventory_id
LEFT JOIN sales_orders so ON sol.so_id = so.so_id

-- Left join for Transit Orders
LEFT JOIN to_inventory_link tol ON i.inventory_id = tol.inventory_id
LEFT JOIN transit_orders tord ON tol.to_id = tord.to_id
LEFT JOIN locations l_to ON tord.to_location_id = l_to.location_id

-- Left join for Work Order Outputs
LEFT JOIN wo_outputs woo ON i.inventory_id = woo.inventory_id
LEFT JOIN work_orders wo_out ON woo.wo_id = wo_out.wo_id

-- Left join for Work Order Inputs
LEFT JOIN wo_input_inventory_link woil ON i.inventory_id = woil.inventory_id
LEFT JOIN work_orders wo_in ON woil.wo_id = wo_in.wo_id;"

"-- Inventory at Point in Time View
CREATE VIEW v_inventory_historical AS
SELECT 
    h.history_id,
    h.inventory_id,
    i.inventory_number,
    h.change_type,
    h.change_timestamp,
    h.field_name,
    h.old_value,
    h.new_value,
    h.change_reason,
    h.transaction_reference,
    h.changed_by,
    si.item_code,
    si.item_description,
    e.entity_name
FROM inventory_history h
INNER JOIN inventory i ON h.inventory_id = i.inventory_id
INNER JOIN stock_items si ON i.item_id = si.item_id
INNER JOIN owning_entities e ON i.entity_id = e.entity_id
ORDER BY h.change_timestamp DESC;"


"-- Stock Level Summary by Item
CREATE VIEW v_stock_levels AS
SELECT 
    si.item_id,
    si.item_code,
    si.item_description,
    l.warehouse,
    qsc.status_name AS quality_status,
    SUM(i.actual_quantity) AS total_quantity,
    SUM(i.allocated_quantity) AS total_allocated,
    SUM(i.actual_quantity - i.allocated_quantity) AS total_available,
    COUNT(DISTINCT i.inventory_id) AS inventory_count
FROM inventory i
INNER JOIN stock_items si ON i.item_id = si.item_id
INNER JOIN locations l ON i.location_id = l.location_id
INNER JOIN quality_status_codes qsc ON i.quality_status_id = qsc.status_id
WHERE i.inventory_status = 'ACTIVE'
GROUP BY si.item_id, si.item_code, si.item_description, l.warehouse, qsc.status_name;"

"-- Add new inventory (receiving)
CREATE PROCEDURE sp_receive_inventory(
    IN p_inventory_number VARCHAR(50),
    IN p_entity_id INT,
    IN p_item_id INT,
    IN p_location_id INT,
    IN p_quantity DECIMAL(15,3),
    IN p_price DECIMAL(15,4),
    IN p_po_id INT,
    IN p_lot_number VARCHAR(100),
    IN p_user VARCHAR(100)
)
BEGIN
    DECLARE v_inventory_id BIGINT;
    
    -- Insert inventory record
    INSERT INTO inventory (
        inventory_number, entity_id, item_id, location_id,
        quality_status_id, current_price, max_quantity,
        planned_quantity, actual_quantity, lot_number,
        entry_date, inventory_status, created_by
    ) VALUES (
        p_inventory_number, p_entity_id, p_item_id, p_location_id,
        1, -- Pending status
        p_price, p_quantity, p_quantity, p_quantity, p_lot_number,
        CURRENT_TIMESTAMP, 'ACTIVE', p_user
    );
    
    SET v_inventory_id = LAST_INSERT_ID();
    
    -- Log history
    INSERT INTO inventory_history (
        inventory_id, change_type, change_timestamp,
        field_name, new_value, changed_by, transaction_reference
    ) VALUES (
        v_inventory_id, 'ENTRY', CURRENT_TIMESTAMP,
        'Initial Entry', 'Item entered stockroom', p_user,
        CONCAT('PO-', p_po_id)
    );
    
    -- Link to PO if provided
    IF p_po_id IS NOT NULL THEN
        INSERT INTO po_inventory_link (po_id, inventory_id, allocated_quantity)
        SELECT p_po_id, v_inventory_id, p_quantity;
    END IF;
    
    SELECT v_inventory_id AS new_inventory_id;
END;"


"-- Update inventory quantity
CREATE PROCEDURE sp_update_inventory_quantity(
    IN p_inventory_id BIGINT,
    IN p_new_quantity DECIMAL(15,3),
    IN p_reason VARCHAR(500),
    IN p_user VARCHAR(100)
)
BEGIN
    DECLARE v_old_quantity DECIMAL(15,3);
    
    SELECT actual_quantity INTO v_old_quantity
    FROM inventory WHERE inventory_id = p_inventory_id;
    
    UPDATE inventory
    SET actual_quantity = p_new_quantity,
        modified_by = p_user,
        modified_date = CURRENT_TIMESTAMP
    WHERE inventory_id = p_inventory_id;
    
    INSERT INTO inventory_history (
        inventory_id, change_type, field_name,
        old_value, new_value, change_reason, changed_by
    ) VALUES (
        p_inventory_id, 'UPDATE', 'actual_quantity',
        v_old_quantity, p_new_quantity, p_reason, p_user
    );
END;"


"-- Insert sample entities
INSERT INTO owning_entities (entity_code, entity_name, entity_type) VALUES
('ACME', 'ACME Manufacturing Ltd', 'Internal'),
('TECH', 'TechCorp Industries', 'Internal'),
('GLOBAL', 'Global Supplies Inc', 'Partner');

-- Insert sample items
INSERT INTO stock_items (item_code, item_description, item_category, unit_of_measure, standard_cost) VALUES
('WIDGET-A100', 'Premium Widget Type A', 'Widgets', 'EA', 100.00),
('COMP-B250', 'Electronic Component B250', 'Electronics', 'EA', 75.00),
('RAW-C300', 'Raw Material C300', 'Raw Materials', 'KG', 25.00);

-- Insert sample locations
INSERT INTO locations (location_code, location_name, warehouse, zone, aisle, shelf, location_type) VALUES
('WH-A-Z1-A3-B2', 'Warehouse A - Zone 1 - Aisle 3 - Shelf B2', 'Warehouse A', 'Zone 1', 'Aisle 3', 'Shelf B2', 'Storage'),
('WH-B-Z1-R5', 'Warehouse B - Zone 1 - Rack 5', 'Warehouse B', 'Zone 1', 'Rack 5', NULL, 'Storage'),
('WH-A-RCV', 'Warehouse A - Receiving', 'Warehouse A', 'Receiving', NULL, NULL, 'Receiving');"

*/
