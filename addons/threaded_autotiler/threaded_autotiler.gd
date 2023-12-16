@tool
extends EditorPlugin

var dock : Control
var button : Button

func _enter_tree():
	dock = preload("res://addons/threaded_autotiler/Scenes/Dock.tscn").instantiate()
	button = add_control_to_bottom_panel(dock, "Autotile")
	button.visible = false
	pass


func _exit_tree():
	remove_control_from_bottom_panel(dock)
	dock.free()
	pass

func _handles(object) -> bool:
	return object is TileMap or object is TileSet

func _make_visible(object) -> void:
	button.visible = true
	pass
	
func _edit(object) -> void:
	if object is TileMap:
		dock.tilemap = object
		dock.tileset = object.tile_set
	if object is TileSet:
		dock.tileset = object
