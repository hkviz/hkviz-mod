namespace HKViz.Silk.Recording;

public partial class PlayerDataWriter(RunFiles runFiles) {

    public void WriteBoolIfChanged(
        ushort fieldId,
        bool oldValue,
        bool newValue
    ) {
        if (oldValue != newValue) {
            runFiles.WritePlayerDataBoolChange(
                fieldId: fieldId,
                value: newValue
            );
        }
    }
}
