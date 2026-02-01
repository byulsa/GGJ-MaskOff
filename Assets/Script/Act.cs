public interface IAct
{
    bool Run(Card card) => true;
    void UpdateCard(Card changedCard); // Detect Card
    void Adjustment(Card card) { }
}
