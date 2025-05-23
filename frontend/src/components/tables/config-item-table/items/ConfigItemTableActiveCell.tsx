import { Switch } from "@/components/ui/switch"
import { publishOnMessageExchange } from "@/lib/hooks/appMessage"
import { CommandUpdateConfigItem } from "@/types/commands"
import { IConfigItem } from "@/types/config"
import { useSortable } from "@dnd-kit/sortable"
import { IconGripVertical } from "@tabler/icons-react"

import { Row } from "@tanstack/react-table"
import React from "react"

interface ConfigItemTableActiveCellProps {
  row: Row<IConfigItem>
}

const ConfigItemTableActiveCell = React.memo(({ row }: ConfigItemTableActiveCellProps) => {
  const { publish } = publishOnMessageExchange()
  const item = row.original as IConfigItem
  const { attributes, listeners } = useSortable({ id: item.GUID })

  return (
    <div className="flex flex-row">
      <div
        {...attributes}
        {...listeners}
        className="cursor-move px-1 text-gray-500 opacity-10 transition-opacity delay-100 ease-in group-hover/row:opacity-100 group-hover/row:delay-100 group-hover/row:ease-out dark:text-gray-300"
      >
        <IconGripVertical className="stroke-2" />
      </div>
      <div className="flex flex-row items-center">
        <Switch
          checked={row.getValue("Active") as boolean}
          onClick={() => {
            item.Active = !item.Active
            publish({
              key: "CommandUpdateConfigItem",
              payload: { item: item },
            } as CommandUpdateConfigItem)
          }}
        />
      </div>
    </div>
  )
})

export default ConfigItemTableActiveCell
